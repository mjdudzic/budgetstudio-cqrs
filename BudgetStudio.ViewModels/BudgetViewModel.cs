using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BudgetStudio.ViewModels
{
	public class BudgetViewModel
	{
		[BsonId]
		[BsonRepresentation(BsonType.String)]
		public Guid BudgetId { get; set; }
		public Guid ProjectId { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? ConfirmedAt { get; set; }
		public DateTime? RejectedAt { get; set; }
		public decimal TotalCostAmount { get; set; }
		public string TotalCostCurrency { get; set; }
		public List<EmployeeCostModel> EmployeeCosts { get; set; }
		public List<ExtraCostModel> ExtraCosts { get; set; }
	}

	public class ExtraCostModel
	{
		public string Description { get; set; }
		public decimal CostAmount { get; set; }
		public string CostCurrency { get; set; }
	}

	public class EmployeeCostModel
	{
		public string EmployeeCode { get; set; }
		public DateTime ParticipationStartedAt { get; set; }
		public DateTime ParticipationEndedAt { get; set; }
		public decimal CostAmount { get; set; }
		public string CostCurrency { get; set; }
	}
}