using System;
using MediatR;

namespace BudgetStudio.Domain.Events
{
	public class BudgetConfirmedEvent : INotification
	{
		public Guid BudgetId { get; }
		public DateTime ConfirmedAt { get; }

		public BudgetConfirmedEvent(Guid budgetId, DateTime confirmedAt)
		{
			BudgetId = budgetId;
			ConfirmedAt = confirmedAt;
		}
	}
}