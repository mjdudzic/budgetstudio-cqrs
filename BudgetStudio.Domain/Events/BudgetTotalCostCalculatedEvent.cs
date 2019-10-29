using System;
using BudgetStudio.Domain.AggregatesModel.BudgetAggregate;
using MediatR;

namespace BudgetStudio.Domain.Events
{
	public class BudgetTotalCostCalculatedEvent : INotification
	{
		public Guid BudgetId { get; }
		public Price TotalCost { get; }

		public BudgetTotalCostCalculatedEvent(
			Guid budgetId,
			Price totalCost)
		{
			BudgetId = budgetId;
			TotalCost = totalCost;
		}
	}
}