using System.Threading;
using System.Threading.Tasks;
using BudgetStudio.Api.Application.BackgroundJobs;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BudgetStudio.Api.Application.Commands
{
	public class RecalculateBudgetCostCommandHandler : IRequestHandler<RecalculateBudgetCostCommand>
	{
		private readonly IBackgroundJobClient _backgroundJobClient;
		private readonly ILogger<RecalculateBudgetCostCommandHandler> _logger;

		public RecalculateBudgetCostCommandHandler(
			IBackgroundJobClient backgroundJobClient,
			ILogger<RecalculateBudgetCostCommandHandler> logger)
		{
			_backgroundJobClient = backgroundJobClient;
			_logger = logger;
		}

		public Task<Unit> Handle(RecalculateBudgetCostCommand command, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Recalculating cost for {budgetId}", command.BudgetId);

			_backgroundJobClient.Enqueue<RecalculateBudgetJob>(
				job => job.Execute(command, null, JobCancellationToken.Null));

			return Task.FromResult(Unit.Value);
		}
	}
}