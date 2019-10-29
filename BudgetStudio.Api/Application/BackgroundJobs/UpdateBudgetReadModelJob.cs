using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BudgetStudio.Api.Application.Constants;
using BudgetStudio.Api.Hubs;
using BudgetStudio.Domain.AggregatesModel.BudgetAggregate;
using BudgetStudio.Infrastructure.Persistence;
using BudgetStudio.ViewModels;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;

namespace BudgetStudio.Api.Application.BackgroundJobs
{
	public class UpdateBudgetReadModelJob : IJob<Guid>
	{
		private readonly IBudgetRepository _budgetRepository;
		private readonly IBudgetNoSqlContext _budgetNoSqlContext;
		private readonly IMemoryCache _memoryCache;
		private readonly IMapper _mapper;
		private readonly IHubContext<NotificationHub> _hubContext;
		private readonly IBudgetNoSqlSettings _budgetNoSqlSettings;

		public UpdateBudgetReadModelJob(
			IBudgetRepository budgetRepository,
			IBudgetNoSqlContext budgetNoSqlContext,
			IMemoryCache memoryCache,
			IMapper mapper,
			IHubContext<NotificationHub> hubContext,
			IBudgetNoSqlSettings budgetNoSqlSettings)
		{
			_budgetRepository = budgetRepository;
			_budgetNoSqlContext = budgetNoSqlContext;
			_memoryCache = memoryCache;
			_mapper = mapper;
			_hubContext = hubContext;
			_budgetNoSqlSettings = budgetNoSqlSettings;
		}

		public async Task Execute(Guid budgetId, PerformContext context, IJobCancellationToken cancellationToken)
		{
			context.WriteLine($"Updating read model for budget {budgetId} started");

			var budget = await _budgetRepository.BudgetForIdAsync(budgetId);

			var viewModel = _mapper.Map<BudgetViewModel>(budget);

			UpdateMemoryCache(viewModel);

			await UpdateNoSqlModel(viewModel, cancellationToken.ShutdownToken);

			await NotifyClientsBudgetDataChanged(viewModel, cancellationToken.ShutdownToken);

			context.WriteLine($"Updating read model for budget {budgetId} completed");
		}

		private void UpdateMemoryCache(BudgetViewModel budgetViewModel)
		{
			_memoryCache.Set(
				CacheKeys.CacheKeyForBudgetId(budgetViewModel.BudgetId),
				budgetViewModel,
				new MemoryCacheEntryOptions
				{
					SlidingExpiration = TimeSpan.FromMinutes(1)
				});
		}

		private async Task UpdateNoSqlModel(BudgetViewModel budgetViewModel, CancellationToken cancellationToken)
		{
			if (_budgetNoSqlSettings.Enabled == false)
			{
				return;
			}

			await _budgetNoSqlContext
				.Budgets
				.ReplaceOneAsync(
					g => g.BudgetId == budgetViewModel.BudgetId,
					budgetViewModel,
					new UpdateOptions { IsUpsert = true },
					cancellationToken);
		}

		private Task NotifyClientsBudgetDataChanged(BudgetViewModel budgetViewModel, CancellationToken cancellationToken)
		{
			return _hubContext.Clients.All.SendAsync(
				"SendNotification",
				"Budget data updated",
				budgetViewModel,
				cancellationToken);
		}
	}
}