using System.Threading.Tasks;
using BudgetStudio.Api.Application.Commands;
using BudgetStudio.Domain.AggregatesModel.BudgetAggregate;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;

namespace BudgetStudio.Api.Application.BackgroundJobs
{
	public class AddEmployeeCostToBudgetJob : IJob<AddEmployeeCostToBudgetCommand>
	{
		private readonly IBudgetRepository _budgetRepository;
		private readonly IEmployeeCostService _employeeCostService;

		public AddEmployeeCostToBudgetJob(
			IBudgetRepository budgetRepository,
			IEmployeeCostService employeeCostService)
		{
			_budgetRepository = budgetRepository;
			_employeeCostService = employeeCostService;
		}

		public async Task Execute(AddEmployeeCostToBudgetCommand command, PerformContext context, IJobCancellationToken cancellationToken)
		{
			context.WriteLine($"Adding employee cost for budget {command.BudgetId} started");

			var budget = await _budgetRepository.BudgetForIdAsync(command.BudgetId);

			await budget.AddEmployeesCost(command.EmployeeIdentity, command.Participation, _employeeCostService);

			await _budgetRepository.SaveChangesAsync(cancellationToken.ShutdownToken);

			context.WriteLine($"Adding employee cost for budget {command.BudgetId} completed");
		}
	}
}