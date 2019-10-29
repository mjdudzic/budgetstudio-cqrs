using System.Threading;
using System.Threading.Tasks;
using BudgetStudio.Api.Application.BackgroundJobs;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BudgetStudio.Api.Application.Commands
{
	public class AddEmployeeCostToBudgetCommandHandler : IRequestHandler<AddEmployeeCostToBudgetCommand>
	{
		private readonly IBackgroundJobClient _backgroundJobClient;
		private readonly ILogger<AddEmployeeCostToBudgetCommandHandler> _logger;

		public AddEmployeeCostToBudgetCommandHandler(
			IBackgroundJobClient backgroundJobClient,
			ILogger<AddEmployeeCostToBudgetCommandHandler> logger)
		{
			_backgroundJobClient = backgroundJobClient;
			_logger = logger;
		}

		public Task<Unit> Handle(AddEmployeeCostToBudgetCommand command, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Adding employee cost for {budgetId}", command.BudgetId);

			_backgroundJobClient.Enqueue<AddEmployeeCostToBudgetJob>(
				job => job.Execute(command, null, JobCancellationToken.Null));

			return Task.FromResult(Unit.Value);
		}
	}
}