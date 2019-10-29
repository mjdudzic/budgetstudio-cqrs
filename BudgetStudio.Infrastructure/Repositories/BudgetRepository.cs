using System;
using System.Threading;
using System.Threading.Tasks;
using BudgetStudio.Domain.AggregatesModel.BudgetAggregate;
using BudgetStudio.Infrastructure.Persistence;

namespace BudgetStudio.Infrastructure.Repositories
{
	public class BudgetRepository : IBudgetRepository
	{
		private readonly BudgetContext _budgetContext;

		public BudgetRepository(BudgetContext budgetContext)
		{
			_budgetContext = budgetContext;
		}

		public async Task<Budget> BudgetForIdAsync(Guid budgetId)
		{
			var budget = await _budgetContext
				.Budgets
				.FindAsync(budgetId);

			if (budget == null)
			{
				return null;
			}

			await _budgetContext
				.Entry(budget)
				.Collection(i => i.EmployeeCosts)
				.LoadAsync();

			await _budgetContext
				.Entry(budget)
				.Collection(i => i.ExtraCosts)
				.LoadAsync();

			return budget;
		}

		public Task AddBudgetAsync(Budget budget)
		{
			_budgetContext.Budgets.Add(budget);

			return Task.CompletedTask;
		}

		public Task SaveChangesAsync(CancellationToken cancellationToken)
		{
			return _budgetContext.SaveEntitiesAsync(cancellationToken);
		}
	}
}