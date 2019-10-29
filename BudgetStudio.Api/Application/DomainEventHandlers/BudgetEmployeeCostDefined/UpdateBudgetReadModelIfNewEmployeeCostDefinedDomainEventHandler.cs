using System.Threading;
using System.Threading.Tasks;
using BudgetStudio.Api.Application.BackgroundJobs;
using BudgetStudio.Domain.Events;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BudgetStudio.Api.Application.DomainEventHandlers.BudgetEmployeeCostDefined
{
	public class UpdateBudgetReadModelIfNewEmployeeCostDefinedDomainEventHandler
		: INotificationHandler<BudgetEmployeeCostDefinedEvent>
	{
		private readonly IBackgroundJobClient _backgroundJobClient;
		private readonly ILogger<UpdateBudgetReadModelIfNewEmployeeCostDefinedDomainEventHandler> _logger;

		public UpdateBudgetReadModelIfNewEmployeeCostDefinedDomainEventHandler(
			IBackgroundJobClient backgroundJobClient,
			ILogger<UpdateBudgetReadModelIfNewEmployeeCostDefinedDomainEventHandler> logger)
		{
			_backgroundJobClient = backgroundJobClient;
			_logger = logger;
		}

		public Task Handle(BudgetEmployeeCostDefinedEvent notification, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Update {budgetId} read model due to new employee cost has been defined", notification.BudgetId);

			_backgroundJobClient.Enqueue<UpdateBudgetReadModelJob>(
				job => job.Execute(notification.BudgetId, null, JobCancellationToken.Null));

			return Task.CompletedTask;
		}
	}
}