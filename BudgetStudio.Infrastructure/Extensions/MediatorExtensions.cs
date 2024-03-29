﻿using System.Linq;
using System.Threading.Tasks;
using BudgetStudio.Domain.SeedWork;
using BudgetStudio.Infrastructure.Persistence;
using MediatR;

namespace BudgetStudio.Infrastructure.Extensions
{
	public static class MediatorExtensions
	{
		public static async Task DispatchDomainEventsAsync(this IMediator mediator, BudgetContext ctx)
		{
			var domainEntities = ctx.ChangeTracker
				.Entries<Entity>()
				.Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
				.ToList();

			var domainEvents = domainEntities
				.SelectMany(x => x.Entity.DomainEvents)
				.ToList();

			domainEntities
				.ForEach(entity => entity.Entity.ClearDomainEvents());

			var tasks = domainEvents
				.Select(async (domainEvent) => {
					await mediator.Publish(domainEvent);
				});

			await Task.WhenAll(tasks);
		}
	}
}