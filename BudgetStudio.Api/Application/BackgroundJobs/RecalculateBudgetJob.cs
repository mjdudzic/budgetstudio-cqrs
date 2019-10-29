using System.Threading.Tasks;
using BudgetStudio.Api.Application.Commands;
using BudgetStudio.Domain.AggregatesModel.BudgetAggregate;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;

namespace BudgetStudio.Api.Application.BackgroundJobs
{
	public class RecalculateBudgetJob : IJob<RecalculateBudgetCostCommand>
	{
		private readonly IBudgetRepository _budgetRepository;
		private readonly ICommissionCalculationService _commissionCalculationService;

		public RecalculateBudgetJob(
			IBudgetRepository budgetRepository,
			ICommissionCalculationService commissionCalculationService)
		{
			_budgetRepository = budgetRepository;
			_commissionCalculationService = commissionCalculationService;
		}

		public async Task Execute(RecalculateBudgetCostCommand command, PerformContext context, IJobCancellationToken cancellationToken)
		{
			context.WriteLine($"Recalculating budget cost for {command.BudgetId} started");

			var budget = await _budgetRepository.BudgetForIdAsync(command.BudgetId);

			await budget.CalculateTotalCost(_commissionCalculationService);

			await _budgetRepository.SaveChangesAsync(cancellationToken.ShutdownToken);

			context.WriteLine($"Recalculating budget cost for {command.BudgetId} completed");
		}
	}
}