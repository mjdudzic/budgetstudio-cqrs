using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BudgetStudio.Infrastructure.Persistence;
using BudgetStudio.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetStudio.Api.Application.Queries
{
	public class GetBudgetTopXByTotalCostQueryHandler : IRequestHandler<GetBudgetTopXByTotalCostQuery, List<BudgetCostReportViewModel>>
	{
		private readonly BudgetContext _budgetContext;

		public GetBudgetTopXByTotalCostQueryHandler(
			BudgetContext budgetContext)
		{
			_budgetContext = budgetContext;
		}

		public Task<List<BudgetCostReportViewModel>> Handle(
			GetBudgetTopXByTotalCostQuery request,
			CancellationToken cancellationToken)
		{
			return _budgetContext
				.Budgets
				.Where(i => i.TotalCost.Currency == request.Currency)
				.OrderByDescending(i => i.TotalCost.Amount)
				.Take(request.CountThreshold)
				.Select(i => new BudgetCostReportViewModel
				{
					BudgetId = i.Id,
					ProjectId = i.ProjectId,
					TotalCostAmount = i.TotalCost.Amount,
					TotalCostCurrency = i.TotalCost.Currency
				})
				.ToListAsync(cancellationToken);
		}
	}
}