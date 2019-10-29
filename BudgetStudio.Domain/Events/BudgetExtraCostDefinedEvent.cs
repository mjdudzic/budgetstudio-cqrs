using System;
using BudgetStudio.Domain.AggregatesModel.BudgetAggregate;
using MediatR;

namespace BudgetStudio.Domain.Events
{
	public class BudgetExtraCostDefinedEvent : INotification
	{
		public Guid BudgetId { get; }
		public ExtraCost ExtraCost { get; }

		public BudgetExtraCostDefinedEvent(
			Guid budgetId,
			ExtraCost extraCost)
		{
			BudgetId = budgetId;
			ExtraCost = extraCost;
		}
	}
}