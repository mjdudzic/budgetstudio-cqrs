using System;
using System.Threading.Tasks;
using BudgetStudio.Api.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace BudgetStudio.Api.Hubs
{
	public class NotificationHub : Hub
	{
		private readonly IMediator _mediator;

		public NotificationHub(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task RecalculateBudget(Guid budgetId)
		{
			await Clients.All.SendAsync(
				"SendNotification", 
				"Budget recalculation started", 
				new { budgetId });

			await _mediator.Send(new RecalculateBudgetCostCommand(budgetId));
		}
	}
}