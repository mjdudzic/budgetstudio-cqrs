using System.Threading;
using System.Threading.Tasks;
using BudgetStudio.Domain.Events;
using BudgetStudio.Api.Application.BackgroundJobs;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BudgetStudio.Api.Application.DomainEventHandlers.BudgetTotalCostCalculated
{
	public class UpdateBudgetReadModelIfTotalCostCalculatedDomainEventHandler
		: INotificationHandler<BudgetTotalCostCalculatedEvent>
	{
		private readonly IBackgroundJobClient _backgroundJobClient;
		private readonly ILogger<UpdateBudgetReadModelIfTotalCostCalculatedDomainEventHandler> _logger;

		public UpdateBudgetReadModelIfTotalCostCalculatedDomainEventHandler(
			IBackgroundJobClient backgroundJobClient,
			ILogger<UpdateBudgetReadModelIfTotalCostCalculatedDomainEventHandler> logger)
		{
			_backgroundJobClient = backgroundJobClient;
			_logger = logger;
		}

		public Task Handle(BudgetTotalCostCalculatedEvent notification, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Update {budgetId} read model due to total cost has been calculated", notification.BudgetId);

			_backgroundJobClient.Enqueue<UpdateBudgetReadModelJob>(
				job => job.Execute(notification.BudgetId, null, JobCancellationToken.Null));

			return Task.CompletedTask;
		}
	}
}