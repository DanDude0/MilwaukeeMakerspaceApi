using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mms.Api.Models;
using Mms.Database;
using Newtonsoft.Json;
using WildApricot;

namespace Mms.Api.Services
{
	public class InvoiceService
	{
		public OperationStatus loadStatus { get; private set; } = new OperationStatus();

		private class DbInvoiceResult
		{
			public int invoice_id { get; set; }
			public int document_number { get; set; }
			public DateTime created_date { get; set; }
			public string contact_name { get; set; }
			public string public_notes { get; set; }
			public string private_notes { get; set; }
			public decimal billed_amount { get; set; }
			public decimal payment_amount { get; set; }
			public decimal paid_amount { get; set; }
			public string notes { get; set; }
		};

		public List<MakersVillageInvoice> ListMVInvoices()
		{
			using var db = new BillingDatabase();
			var sql = @"
				SELECT 
					makers_village_invoice_id AS Id, 
					title AS Title, 
					total_mv_paid AS TotalMvPaid,
					total_mv_outstanding AS TotalMvOutstanding, 
					created_date AS CreatedDate
				FROM 
					makers_village_invoice 
				ORDER BY
					created_date DESC";

			var list = db.Fetch<MakersVillageInvoice>(sql);

			return list;
		}

		public async Task<object> GenerateMVInvoice(int year, int month)
		{
			// Don't use .AddMonth(), gets weird with the 28/29/30/31.
			var endYear = year;
			var endMonth = month + 1;

			if (month == 12) {
				endYear = year + 1;
				endMonth = 1;
			}

			var startDate = new DateTime(year, month, 1);
			var endDate = new DateTime(endYear, endMonth, 1);
			var invoice = new MakersVillageInvoice();

			invoice.Title = $"Makers Village Sublet Invoice - {year}-{month:00}";
			invoice.Start = startDate;
			invoice.End = endDate;
			invoice.CreatedDate = DateTime.Now;

			using var db = new BillingDatabase();
			var sql = @"
				SELECT
					i.invoice_id, 
					i.document_number, 
					i.created_date,
					i.contact_name,
					i.public_notes,
					i.private_notes,
					i.amount AS billed_amount,
					p.amount AS payment_amount,
					i.paid_amount AS paid_amount,
					s.notes
				FROM 
					invoice i 
					INNER JOIN invoice_line il 
						ON i.invoice_id = il.invoice_id
					LEFT JOIN (
						SELECT 
							invoice_id, 
							SUM(amount) AS amount
						FROM 
							payment_allocation 
						WHERE
							payment_date >= @0
							AND payment_date < @1
						GROUP BY 
							invoice_id) p
						ON i.invoice_id = p.invoice_id
					LEFT JOIN (
						SELECT 
							invoice_id, 
							notes 
						FROM 
							storage_notes 
						GROUP BY 
							invoice_id 
						ORDER BY 
							snapshot_date ASC) s
						ON i.invoice_id = s.invoice_id					
				WHERE
					il.notes LIKE '%Makers Village%'
					AND (p.amount > 0
						OR i.paid_amount < i.amount)
					AND voided_date < '2000-01-01'
				GROUP BY
					i.invoice_id
				ORDER BY
					i.invoice_date ASC";

			var members = await db.FetchAsync<DbInvoiceResult>(sql, startDate, endDate);

			invoice.Invoices = new List<MakersVillageInvoice.MemberInvoice>(members.Count);

			foreach (var item in members) {
				var member = new MakersVillageInvoice.MemberInvoice {
					Id = item.document_number,
					Created = item.created_date,
					Name = item.contact_name,
					PublicNotes = item.public_notes,
					PrivateNotes = item.private_notes,
					MmsBilled = item.billed_amount,
					MmsPrePaid = item.paid_amount - item.payment_amount,
					MmsPaid = item.payment_amount,
					MmsOutstanding = item.billed_amount - item.paid_amount,
					StorageNotes = item.notes,
				};

				var sql2 = @"
					SELECT 
						notes, 
						amount
					FROM 
						invoice_line
					WHERE
						invoice_id = @0
						AND notes LIKE '%Makers Village%'
					ORDER BY
						invoice_line_id ASC";

				var lines = await db.FetchAsync<MakersVillageInvoice.InvoiceLine>(sql2, item.invoice_id);

				foreach (var line in lines) {
					member.MvOwed += Math.Round(line.Amount * 0.96m, 2);
				}

				member.Lines = lines;
				member.MvPrePaid = Math.Round(member.MvOwed * member.MmsPrePaid / member.MmsBilled, 2);
				member.MvPaid = Math.Round(member.MvOwed * member.MmsPaid / member.MmsBilled, 2);
				member.MvOutstanding = Math.Round(member.MvOwed * member.MmsOutstanding / member.MmsBilled, 2);
				invoice.TotalMmsBilled += member.MmsBilled;
				invoice.TotalMmsPrePaid += member.MmsPrePaid;
				invoice.TotalMmsPaid += member.MmsPaid;
				invoice.TotalMmsOutstanding += member.MmsOutstanding;
				invoice.TotalMvOwed += member.MvOwed;
				invoice.TotalMvPrePaid += member.MvPrePaid;
				invoice.TotalMvPaid += member.MvPaid;
				invoice.TotalMvOutstanding += member.MvOutstanding;
				invoice.Invoices.Add(member);
			}

			var sql3 = @"
					SELECT 
						makers_village_invoice_adjustments_id,
						date,
						reason,
						amount
					FROM 
						makers_village_invoice_adjustments
					WHERE
						date >= @0
						AND date < @1
					ORDER BY
						date ASC";

			var adjustments = await db.FetchAsync<MakersVillageInvoice.Adjustment>(sql3, startDate, endDate);

			foreach (var adjustment in adjustments) {
				invoice.TotalMvAdjustments += adjustment.Amount;
				invoice.TotalMvPaid += adjustment.Amount;
			}

			invoice.Adjustments = adjustments;

			var serializedInvoice = JsonConvert.SerializeObject(invoice);

			var dbInvoice = new makers_village_invoice {
				title = invoice.Title,
				year = year,
				month = month,
				total_mms_billed = invoice.TotalMmsBilled,
				total_mms_prepaid = invoice.TotalMmsPrePaid,
				total_mms_paid = invoice.TotalMmsPaid,
				total_mms_outstanding = invoice.TotalMmsOutstanding,
				total_mv_owed = invoice.TotalMvOwed,
				total_mv_prepaid = invoice.TotalMvPrePaid,
				total_mv_adjustments = invoice.TotalMvAdjustments,
				total_mv_paid = invoice.TotalMvPaid,
				total_mv_outstanding = invoice.TotalMvOutstanding,
				created_date = invoice.CreatedDate,
				details = serializedInvoice,
			};

			var result = await db.InsertAsync(dbInvoice);

			return result;
		}

		public async Task Load(DateTimeOffset start, DateTimeOffset end)
		{
			var wildApricot = new WildApricotClient();

			loadStatus.range = 1;
			loadStatus.progress = 0;
			loadStatus.status = "Loading Invoice List";

			// Pad the loading, because of time zone offset stupidity.
			var invoiceIdList = await wildApricot.GetInvoicesListAsync(
				accountId: wildApricot.accountId,
				startDate: start.AddDays(-1),
				endDate: end.AddDays(1),
				idsOnly: true
				);

			loadStatus.status = "Loading Voidable Invoices";

			var sql = @"
				SELECT
					invoice_id
				FROM
					invoice
				WHERE
					is_paid = 0
					AND created_date > @0
					AND voided_date < '2000-01-01'";

			using var db = new BillingDatabase();
			var voidableInvoices = await db.FetchAsync<int>(sql, start.AddMonths(-2));

			foreach (var voidableId in voidableInvoices) {
				if (!invoiceIdList.InvoiceIdentifiers.Contains(voidableId)) {
					invoiceIdList.InvoiceIdentifiers.Add(voidableId);
					loadStatus.range += 1;
				}

			}

			loadStatus.status = "Loading Payment List";

			var paymentIdList = await wildApricot.GetPaymentsListAsync(
				accountId: wildApricot.accountId,
				startDate: start.AddDays(-1),
				endDate: end.AddDays(1),
				idsOnly: true
				);

			loadStatus.range = invoiceIdList.InvoiceIdentifiers.Count + paymentIdList.PaymentIdentifiers.Count;

			var count = 0;

			foreach (var item in paymentIdList.PaymentIdentifiers) {
				loadStatus.progress += 1;
				count += 1;
				loadStatus.status = $"Loading Payment {count} of {paymentIdList.PaymentIdentifiers.Count}";

				var waPaymentAllocations = await wildApricot.GetPaymentAllocationsListAsync(wildApricot.accountId, paymentId: item);

				foreach (var waAllocation in waPaymentAllocations) {
					if (waAllocation.Invoice?.Id == null)
						continue;

					if (waAllocation.InvoiceDate < start) {
						var id = waAllocation.Invoice.Id ?? -1;

						if (!invoiceIdList.InvoiceIdentifiers.Contains(id)) {
							invoiceIdList.InvoiceIdentifiers.Add(id);
							loadStatus.range += 1;
						}
					}

					var dbPaymentAllocation = new payment_allocation {
						payment_allocation_id = waAllocation.Id.Value,
						amount = (decimal)waAllocation.Value.Value,
						invoice_id = waAllocation.Invoice.Id.Value,
						payment_id = waAllocation.Payment.Id.Value,
						payment_date = waAllocation.PaymentDate.Value.LocalDateTime,
					};

					db.Delete<payment_allocation>(dbPaymentAllocation.payment_allocation_id);
					db.Insert(dbPaymentAllocation);
				}
			}

			count = 0;

			foreach (var item in invoiceIdList.InvoiceIdentifiers) {
				loadStatus.progress += 1;
				count += 1;
				loadStatus.status = $"Loading Invoice Heading {count} of {invoiceIdList.InvoiceIdentifiers.Count}";

				var waInvoice = await wildApricot.GetInvoiceDetailsAsync(wildApricot.accountId, item);

				var dbInvoice = new invoice {
					invoice_id = waInvoice.Id.Value,
					document_number = waInvoice.DocumentNumber,
					invoice_date = waInvoice.DocumentDate.Value.LocalDateTime,
					amount = (decimal)waInvoice.Value.Value,
					paid_amount = (decimal)waInvoice.PaidAmount.Value,
					is_paid = (sbyte)((waInvoice.IsPaid ?? false) ? 1 : 0),
					type = waInvoice.OrderType.ToString(),
					private_notes = waInvoice.Memo ?? "",
					public_notes = waInvoice.PublicMemo ?? "",
					created_date = waInvoice.CreatedDate.Value.LocalDateTime,
					updated_date = waInvoice.UpdatedDate.GetValueOrDefault().LocalDateTime,
					voided_date = waInvoice.VoidedDate.GetValueOrDefault().LocalDateTime,
					contact_id = waInvoice.Contact.Id.Value,
					contact_name = waInvoice.Contact.Name,
					creator_id = waInvoice.CreatedBy?.Id ?? -1,
					updater_id = waInvoice.UpdatedBy?.Id ?? -1,
				};

				db.Delete<invoice>(dbInvoice.invoice_id);
				db.Insert(dbInvoice);

				var i = 0;
				var makersVillageInvoice = false;

				foreach (var waLine in waInvoice.OrderDetails) {
					i += 1;
					loadStatus.status = $"Loading Invoice Lines {count} of {loadStatus.range}, Line {i}";

					var dbLine = new invoice_line {
						invoice_line_id = ((long)dbInvoice.invoice_id * 100) + i,
						invoice_id = dbInvoice.invoice_id,
						amount = (decimal)waLine.Value.Value,
						type = waLine.OrderDetailType.ToString(),
						notes = waLine.Notes,
					};

					db.Delete<invoice_line>(dbLine.invoice_line_id);
					db.Insert(dbLine);

					if (dbLine.notes.Contains("Makers Village"))
						makersVillageInvoice = true;
				}

				if (makersVillageInvoice) {
					var waContact = await wildApricot.GetContactDetailsAsync(wildApricot.accountId, (int)dbInvoice.contact_id);
					var note = "";

					foreach (var field in waContact.FieldValues) {
						if (field.FieldName == "Makers Village Storage Description") {
							note = field.Value.ToString();

							break;
						}
					}

					var dbNote = new storage_note {
						invoice_id = dbInvoice.invoice_id,
						contact_id = dbInvoice.contact_id,
						snapshot_date = DateTime.Now,
						notes = note,
					};

					db.Insert(dbNote);
				}
			}

			loadStatus.range = 0;
			loadStatus.progress = 0;
			loadStatus.status = "Load Completed";
		}
	}
}
