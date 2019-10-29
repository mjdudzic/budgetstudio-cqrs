using System;

namespace BudgetStudio.Api.Application.Constants
{
	public static class CacheKeys
	{
		public static string BudgetCacheKeySuffix => "_Budget";

		public static string CacheKeyForBudgetId(Guid budgetId)
			=> $"{BudgetCacheKeySuffix}_{budgetId}";
	}
}