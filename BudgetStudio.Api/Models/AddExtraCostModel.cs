using System;
using System.ComponentModel.DataAnnotations;

namespace BudgetStudio.Api.Models
{
	public class AddExtraCostModel
	{
		[Required]
		public string Description { get; set; }
		[Required]
		public decimal CostAmount { get; set; }
		[Required]
		public string CostCurrency { get; set; }
	}
}