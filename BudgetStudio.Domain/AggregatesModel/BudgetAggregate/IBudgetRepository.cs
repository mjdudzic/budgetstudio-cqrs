using System;
using System.Threading;
using System.Threading.Tasks;

namespace BudgetStudio.Domain.AggregatesModel.BudgetAggregate
{
	public interface IBudgetRepository
	{
		Task<Budget> BudgetForIdAsync(Guid budgetId);
		Task AddBudgetAsync(Budget budget);
		Task SaveChangesAsync(CancellationToken cancellationToken);
	}
}