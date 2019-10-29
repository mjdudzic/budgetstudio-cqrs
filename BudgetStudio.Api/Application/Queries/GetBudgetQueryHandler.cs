using System;
using System.Threading;
using System.Threading.Tasks;
using BudgetStudio.Api.Application.Constants;
using BudgetStudio.ViewModels;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;

namespace BudgetStudio.Api.Application.Queries
{
	public class GetBudgetQueryHandler : IRequestHandler<GetBudgetQuery, BudgetViewModel>
	{
		private readonly IMemoryCache _memoryCache;
		private readonly IBudgetNoSqlContext _budgetNoSqlContext;

		public GetBudgetQueryHandler(
			IMemoryCache memoryCache,
			IBudgetNoSqlContext budgetNoSqlContext)
		{
			_memoryCache = memoryCache;
			_budgetNoSqlContext = budgetNoSqlContext;
		}

		public async Task<BudgetViewModel> Handle(GetBudgetQuery request, CancellationToken cancellationToken)
		{
			if (_memoryCache.TryGetValue(CacheKeys.CacheKeyForBudgetId(request.BudgetId), out BudgetViewModel cacheEntry))
			{
				return cacheEntry;
			}

			var filter = Builders<BudgetViewModel>.Filter.Eq(m => m.BudgetId, request.BudgetId);
			
			var budgetViewModel = await _budgetNoSqlContext
				.Budgets
				.Find(filter)
				.FirstOrDefaultAsync(cancellationToken);

			if (budgetViewModel == null)
			{
				return null;
			}

			_memoryCache.Set(
				CacheKeys.CacheKeyForBudgetId(budgetViewModel.BudgetId),
				budgetViewModel,
				new MemoryCacheEntryOptions
				{
					SlidingExpiration = TimeSpan.FromMinutes(1)
				});

			return budgetViewModel;
		}
	}
}