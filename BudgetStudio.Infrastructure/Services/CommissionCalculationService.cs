using System;
using System.Threading.Tasks;
using BudgetStudio.Domain.AggregatesModel.BudgetAggregate;

namespace BudgetStudio.Infrastructure.Services
{
	public class CommissionCalculationService : ICommissionCalculationService
	{
		public async Task<Price> CalculateCommissionCostAsync(Guid projectId, Price budgetCost)
		{
			await Task.Delay(0);

			var commission = budgetCost.Amount * 0.2m;

			return new Price(commission, "PLN");
		}
	}
}