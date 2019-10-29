using System;
using System.ComponentModel.DataAnnotations;

namespace BudgetStudio.ViewModels
{
	public class BudgetOdataViewModel
	{
		[Key]
		public Guid BudgetId { get; set; }
		public Guid ProjectId { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? ConfirmedAt { get; set; }
		public DateTime? RejectedAt { get; set; }
		public string RejectionReason { get; set; }
		public decimal TotalCostAmount { get; set; }
		public string TotalCostCurrency { get; set; }
		public int EmployeesCount { get; set; }
		public decimal EmployeesTotalCost { get; set; }
		public int ExtraCostsCount { get; set; }
		public decimal ExtraCostsTotalCost { get; set; }
	}
}