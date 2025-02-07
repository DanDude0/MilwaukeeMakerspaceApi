using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using CronScheduler.Extensions.Scheduler;
using Mms.Database;
using PetaPoco;
using static SQLite.SQLite3;
using WildApricot;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Policy;
using Serilog;

namespace Mms.Api.Jobs
{
	public class PullMembersFromWildApricot : IScheduledJob
	{
		public string Name { get; } = nameof(PullMembersFromWildApricot);

		public async Task ExecuteAsync(CancellationToken cancellationToken)
		{
			Log.Information("Beginning Wild Apricot Sync");

			var wildApricot = new WildApricotClient();

			var contactsCount = await wildApricot.GetContactsListAsync(
				accountId: wildApricot.accountId,
				//count: true,
				filter: "Archived eq false"
				);

			ContactsResponse contacts = null;

			var attempts = 0;

			while (attempts < 10 && string.IsNullOrWhiteSpace(contacts?.Processed) && contacts?.ProcessingState != ContactsAsyncResponseProcessingState.Complete) {
				await Task.Delay(5000);

				contacts = await wildApricot.GetContactsListAsync(
					accountId: wildApricot.accountId,
					resultId: contactsCount.ResultId
					);

				attempts += 1;
			}

			if ((string.IsNullOrWhiteSpace(contacts?.Processed) && contacts?.ProcessingState != ContactsAsyncResponseProcessingState.Complete) || contacts?.Contacts?.Count < 101)
				throw new Exception("Wild Apricot did not return complete members list!");

			using var accessDb = new AccessControlDatabase();
			using var fundingDb = new AreaFundingDatabase();

			Log.Information($"Contacts Returned: {contacts.Contacts.Count}");

			var totalFunding = new fund();

		foreach (var contact in contacts.Contacts) {
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
				var amount = 0m;

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
							break;
						case "Member role":
							switch (field.Value?.ToString() ?? "") {
								case "Bundle Administrator":
								case "Bundle administrator":
								case "Individual":
								case "":
									type = "General";

									if (active) {
										totalFunding.general += 1;
										amount = 1.5m;
									}
									break;
								case "Bundle Member":
								case "Bundle member":
									type = "Family";

									if (active) {
										totalFunding.family += 1;
										amount = 0.38m;
									}
									break;
							}
							break;
						case "Suspended member":
						case "Archived":
							// If the BOD suspends someone before they expire, we immediately disable their key
							if (field.Value?.ToString() == "1") {
								active = false;
								var sql = @"
									UPDATE
										member
									SET
										expires = '2000-01-01'
									WHERE
										member_id = @0;";

								accessDb.Execute(sql, id);
							}
							break;
						case "$1.50/Month Area #1":
						case "$1.50/Month Area #2":
						case "$1.50/Month Area #3":
						case "$1.50/Month Area #4":
						case "$1.50/Month Area #5":
							if (active) {
								if (field.Value == null)
									Log.Error($"Null Area Selection! - {fullname} - {field.FieldName} - {field.Value}");
								else {
									TabulateFunds(totalFunding, field.FieldName, amount);
									totalFunding.total += amount;
								}
							}
							break;
					}
				}

				if (active)
					totalFunding.members += 1;

				if (specialPurpose) {
					// Always set special purpose accounts to expire one month into the future.
					renewal = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)).AddMonths(1);
				}

				// Record keys into access database
				if (active || specialPurpose) {
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
			}

			totalFunding.month = DateTime.Now;

			fundingDb.Insert(totalFunding);
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
				case "Ham Radio":
					total.ham_radio += amount;
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
				case "Neon":
					total.neon += amount;
					break;
				case "Paint Room":
					total.paint += amount;
					break;
				case "Stained Glass":
					total.stained_glass += amount;
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
