using System;
using BudgetStudio.ViewModels;
using MediatR;

namespace BudgetStudio.Api.Application.Queries
{
	public class GetBudgetQuery : IRequest<BudgetViewModel>
	{
		public Guid BudgetId { get; }

		public GetBudgetQuery(Guid budgetId)
		{
			BudgetId = budgetId;
		}
	}
}