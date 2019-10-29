using System;

namespace BudgetStudio.ViewModels
{
	public class BudgetCostReportViewModel
	{
		public Guid BudgetId { get; set; }
		public Guid ProjectId { get; set; }
		public decimal TotalCostAmount { get; set; }
		public string TotalCostCurrency { get; set; }
	}
}