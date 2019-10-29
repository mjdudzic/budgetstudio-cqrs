using Hangfire.Dashboard;

namespace BudgetStudio.Api.Filters
{
	public class FakeDashboardAuthFilter : IDashboardAuthorizationFilter
	{
		public bool Authorize(DashboardContext context)
		{
			return true;
		}
	}
}