using System;
using MediatR;

namespace BudgetStudio.Domain.Events
{
	public class BudgetRejectedEvent : INotification
	{
		public Guid BudgetId { get; }
		public DateTime RejectedAt { get; }
		public string RejectionReason { get; }

		public BudgetRejectedEvent(Guid budgetId, DateTime rejectedAt, string rejectionReason)
		{
			BudgetId = budgetId;
			RejectedAt = rejectedAt;
			RejectionReason = rejectionReason;
		}
	}
}