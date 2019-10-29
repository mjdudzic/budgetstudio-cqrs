using System;
using System.Threading.Tasks;

namespace BudgetStudio.Domain.AggregatesModel.BudgetAggregate
{
	public interface ICommissionCalculationService
	{
		Task<Price> CalculateCommissionCostAsync(Guid projectId, Price budgetCost);
	}
}