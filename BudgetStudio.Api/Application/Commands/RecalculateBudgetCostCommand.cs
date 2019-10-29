using System;
using MediatR;

namespace BudgetStudio.Api.Application.Commands
{
	public class RecalculateBudgetCostCommand : IRequest
	{
		public Guid BudgetId { get; }

		public RecalculateBudgetCostCommand(
			Guid budgetId)
		{
			BudgetId = budgetId;
		}
	}
}