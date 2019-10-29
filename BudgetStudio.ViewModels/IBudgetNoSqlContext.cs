using MongoDB.Driver;

namespace BudgetStudio.ViewModels
{
	public interface IBudgetNoSqlContext
	{
		IMongoCollection<BudgetViewModel> Budgets { get; }
	}
}