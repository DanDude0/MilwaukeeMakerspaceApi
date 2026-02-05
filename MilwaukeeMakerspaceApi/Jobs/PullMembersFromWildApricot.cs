using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using CronScheduler.Extensions.Scheduler;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Mms.Database;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PetaPoco;
using Serilog;
using WildApricot;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static SQLite.SQLite3;

namespace Mms.Api.Jobs
{
	public class PullMembersFromWildApricot : IScheduledJob
	{
		public string Name { get; } = nameof(PullMembersFromWildApricot);

		public async Task ExecuteAsync(CancellationToken cancellationToken)
		{
			Log.Information("Beginning Wild Apricot Sync");

			var wildApricot = new WildApricotClient();

			var contacts = new List<Contact>();

			var maxLoadLength = 100;
			var skip = 0;
			var loadLength = 0;

#if DEBUG
			if (System.IO.File.Exists("cache.json")) {
				Log.Fatal($"Loading previous version of contacts from local cache.json, to avoid getting banned by apricot API for too many hits.");

				contacts = JsonConvert.DeserializeObject<List<Contact>>(System.IO.File.ReadAllText("cache.json"));
			}
			else {
#endif

				Log.Debug($"Connecting to Wild Apricot API");

				do {
					// Pad the loading, because of time zone offset stupidity.
					var loadResponse = await wildApricot.GetContactsListAsync(
					accountId: wildApricot.accountId,
					filter: "Archived eq false",
					async: false,
					top: maxLoadLength,
					skip: skip
					);

					loadLength = loadResponse.Contacts.Count;
					contacts.AddRange(loadResponse.Contacts);
					skip += maxLoadLength;

					Log.Debug($"Loaded {skip} contacts...");
				} while (loadLength == maxLoadLength);

				if (contacts.Count < 1)
					throw new Exception("Wild Apricot did not return members list!");

#if DEBUG
				System.IO.File.WriteAllText("cache.json", JsonConvert.SerializeObject(contacts));
				Log.Fatal($"Stored new local copy of contacts to local cache.json, to avoid getting banned by apricot API for too many hits. Delete this file to try with new data.");
			}
#endif

			Log.Information($"Contacts Returned: {contacts.Count}");

			var totalFunding = new fund();

			var accessDb = new AccessControlDatabase();

			for (int count = 0; count < contacts.Count; count += 1) {
				var contact = contacts[count];

				// For Keys
				string key1 = null;
				string key2 = null;
				string type = null;
				DateTime? joined = null;
				DateTime? renewal = null;
				var id = contact.Id ?? -1;
				var key3 = $"{id}#";
				var fullname = $"{contact.FirstName} {contact.LastName}";
				var apricot_admin = contact.IsAccountAdministrator ?? false;

				// For Funding
				var active = false;
				var specialPurpose = false;
				var forceExpire = false;
				var amount = 0m;
				var memberFunding = new List<string>();

				// If a member is currently pending renewal (during last 10 days of month, for instance), we count them as active
				switch (contact.Status ?? ContactStatus.Lapsed) {
					case ContactStatus.Active:
					case ContactStatus.PendingRenewal:
						active = true;
						break;
				}

				// If an account is active, and set to the type "Special Purpose Account" 
				// we do not count it towards the population or area funding.
				// We do put a key in the access control system for it.
				// Expiration date is overridden below.
				if (contact.MembershipLevel?.Name == "Special Purpose Account") {
					active = false;
					specialPurpose = true;
				}
				else if (contact.MembershipLevel?.Name == "Self sign up") {
					active = false;
					specialPurpose = false;
				}

				foreach (var field in contact.FieldValues) {
					switch (field.FieldName) {
						case "Key Fob Code":
							key1 = field.Value?.ToString().ToUpperInvariant().Trim();
							break;
						case "PIN Code":
							key2 = field.Value?.ToString().ToUpperInvariant().Trim();
							break;
						case "Member since":
							joined = DateTime.Parse(field.Value?.ToString() ?? "2000-01-01");
							break;
						case "Renewal due":
							renewal = DateTime.Parse(field.Value?.ToString() ?? "2000-01-01");

							var gracePeriod = DateTime.Now.AddDays(-7);

							if (renewal < gracePeriod)
								active = false;
							break;
						case "Member role":
							var rawType = field.Value?.ToString().ToLowerInvariant() ?? "";

							switch (rawType) {
								case "bundle administrator":
								case "individual":
								case "":
									type = "General";
									break;
								case "bundle member":
									type = "Family";
									break;
								default:
									// Copy it now for debugging, but expect this to blow up later.
									type = rawType;
									break;
							}
							break;
						case "Suspended member":
						case "Archived":
							// If the BOD suspends someone before they expire, we immediately disable their key
							if (field.Value?.ToString().ToLowerInvariant() == "true") {
								active = false;
								forceExpire = true;
							}
							break;
						case "$1.50/Month Area #1":
						case "$1.50/Month Area #2":
						case "$1.50/Month Area #3":
						case "$1.50/Month Area #4":
						case "$1.50/Month Area #5":
							if (field.Value != null) {
								var fieldContents = JsonDocument.Parse(field.Value.ToString()).RootElement;

								memberFunding.Add(fieldContents.GetProperty("Label").GetString());
							}
							break;
					}
				}

				if (specialPurpose) {
					// Always set special purpose accounts to expire one month into the future.
					renewal = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)).AddMonths(1);
				}

				// Update Database with current information about member.
				try {
					// Record keys into access database
					if (forceExpire) {
						var sql = @"
									UPDATE
										member
									SET
										expires = '2000-01-01'
									WHERE
										member_id = @0;";

						accessDb.Execute(sql, id);

						Log.Debug($"Forcing Expiration of '{fullname}' from database.");
					}
					else if (active || specialPurpose) {
						Log.Debug($"Recording '{fullname}' in database.");

						if (!string.IsNullOrEmpty(key1)) {
							var sql2 = @$"
								REPLACE INTO
									keycode(keycode_id, member_id, updated)
								VALUES
									(@0, @1, NOW()); ";

							accessDb.Execute(sql2, key1, id);
						}

						if (!string.IsNullOrEmpty(key2)) {
							var sql3 = @$"
								REPLACE INTO
									keycode(keycode_id, member_id, updated)
								VALUES
									(@0, @1, NOW()); ";

							accessDb.Execute(sql3, key2, id);
						}

						var sql4 = @$"
							REPLACE INTO
								keycode(keycode_id, member_id, updated)
							VALUES
									(@0, @1, NOW()); ";

						accessDb.Execute(sql4, key3, id);

						var sql5 = @"
							REPLACE INTO
								member(member_id, name, type, apricot_admin, joined, expires, updated)
							VALUES
								(@0, @1, @2, @3, @4, @5, NOW()); ";

						accessDb.Execute(sql5, id, fullname, type, apricot_admin, joined, renewal);
					}
					else
						Log.Debug($"Excluding '{fullname}' from database.");

					// Only update totals if database update was successful.
					if (active) {
						totalFunding.members += 1;

						if (type == "Family") {
							totalFunding.family += 1;
							amount = 0.38m;
						}
						else if (type == "General") {
							totalFunding.general += 1;
							amount = 1.50m;
						}
						else {
							Log.Error($"Unknown member type! - '{fullname}' - '{type}'");
						}

						var fundCount = memberFunding.Count;

						if (fundCount < 5)
							Log.Error($"Recorded too few '{fundCount}' Area Funding selections for '{fullname}'");
						else if (fundCount > 5)
							Log.Fatal($"Recorded too many '{fundCount}' Area Funding selections for '{fullname}'");

						foreach (var area in memberFunding) {
							TabulateFunds(totalFunding, area, amount);
							totalFunding.total += amount;
						}
					}
				}
				catch (Exception ex) {
					Log.Error(ex, "Database Error while importing contacts");

					accessDb.Dispose();
					accessDb = new AccessControlDatabase();

					// This happens with some regularity, given that there are thousands of queries executed in this loop. Reconnecting fixes it.
					// Backup and try again.
					count -= 1;
				}
			}

			Log.Information("Finished Parsing Contacts");

			accessDb?.Dispose();

			totalFunding.month = DateTime.Now;

			using var fundingDb = new AreaFundingDatabase();

			fundingDb.Insert(totalFunding);

			Log.Information("Finished Wild Apricot Sync");
		}

		private void TabulateFunds(fund total, string area, decimal amount)
		{
			switch (area) {
				case "* Not Allocated *":
					total.building_purchase += amount;
					break;
				case "3D Printers":
					total.threed_printer += amount;
					break;
				case "Bicycle Repair":
					total.bicycle_repair += amount;
					break;
				case "Blacksmith and Forge":
					total.forge += amount;
					break;
				case "Casting":
					total.casting += amount;
					break;
				case "Ceramic Studio":
					total.ceramic += amount;
					break;
				case "CNC Room":
					total.cnc += amount;
					break;
				case "Cosplay":
					total.cosplay += amount;
					break;
				case "Craft Lab":
					total.craft += amount;
					break;
				case "Dalek Asylum":
					total.dalek += amount;
					break;
				case "Electronic Lab":
					total.electronic += amount;
					break;
				case "Fine Art Printing":
					total.print += amount;
					break;
				case "Finishing":
					total.finishing += amount;
					break;
				case "Glass Fusing":
					total.glass_fusing += amount;
					break;
				case "Ham Radio":
					total.ham_radio += amount;
					break;
				case "Hand Wood Carving":
					total.hand_wood_carving += amount;
					break;
				case "Jewelry and Non Ferrous Metals":
					total.jewelry += amount;
					break;
				case "Lampworking":
					total.lampworking += amount;
					break;
				case "Laser Cutters":
					total.laser += amount;
					break;
				case "Leather Working":
					total.leather += amount;
					break;
				case "Long Arm Quilting":
					total.long_arm += amount;
					break;
				case "Community Outreach":
					total.makerfaire += amount;
					break;
				case "Metal Shop":
					total.metal += amount;
					break;
				case "Model and Miniatures":
					total.models += amount;
					break;
				case "Neon":
					total.neon += amount;
					break;
				case "Paint Room":
					total.paint += amount;
					break;
				case "Small Engine":
					total.small_engine += amount;
					break;
				case "Stained Glass":
					total.stained_glass += amount;
					break;
				case "Sublimation":
					total.sublimation += amount;
					break;
				case "Tiger Lily Sculpture Gang":
					total.tiger_lily += amount;
					break;
				case "Vacuum Former":
					total.vacuum += amount;
					break;
				case "Welders":
					total.welding += amount;
					break;
				case "Wood Shop":
					total.wood += amount;
					break;
			}
		}
	}
}
