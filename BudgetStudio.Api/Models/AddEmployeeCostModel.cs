using System;
using System.ComponentModel.DataAnnotations;

namespace BudgetStudio.Api.Models
{
	public class AddEmployeeCostModel
	{
		[Required]
		public string EmployeeCode { get; set; }
		[Required]
		public DateTime StartedAt { get; set; }
		[Required]
		public DateTime EndedAt { get; set; }
	}
}