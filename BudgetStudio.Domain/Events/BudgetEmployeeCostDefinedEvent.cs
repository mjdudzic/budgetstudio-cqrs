using System;
using BudgetStudio.Domain.AggregatesModel.BudgetAggregate;
using MediatR;

namespace BudgetStudio.Domain.Events
{
	public class BudgetEmployeeCostDefinedEvent : INotification
	{
		public Guid BudgetId { get; }
		public EmployeeCost EmployeeCost { get; }

		public BudgetEmployeeCostDefinedEvent(
			Guid budgetId,
			EmployeeCost employeeCost)
		{
			BudgetId = budgetId;
			EmployeeCost = employeeCost;
		}
	}
}