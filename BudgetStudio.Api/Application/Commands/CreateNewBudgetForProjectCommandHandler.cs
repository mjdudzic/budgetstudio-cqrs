using System;
using System.Threading;
using System.Threading.Tasks;
using BudgetStudio.Domain.AggregatesModel.BudgetAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BudgetStudio.Api.Application.Commands
{
	public class CreateNewBudgetForProjectCommandHandler : IRequestHandler<CreateNewBudgetForProjectCommand, Guid>
	{
		private readonly IBudgetRepository _budgetRepository;
		private readonly ILogger<CreateNewBudgetForProjectCommandHandler> _logger;

		public CreateNewBudgetForProjectCommandHandler(
			IBudgetRepository budgetRepository,
			ILogger<CreateNewBudgetForProjectCommandHandler> logger)
		{
			_budgetRepository = budgetRepository;
			_logger = logger;
		}

		public async Task<Guid> Handle(CreateNewBudgetForProjectCommand command, CancellationToken cancellationToken)
		{
			var budgetId = Guid.NewGuid();

			_logger.LogInformation("Adding new budget {budgetId} for {projectId}", budgetId, command.ProjectId);

			var budget = new Budget(
				budgetId,
				command.ProjectId);

			await _budgetRepository.AddBudgetAsync(budget);

			await _budgetRepository.SaveChangesAsync(cancellationToken);

			return budgetId;
		}
	}
}