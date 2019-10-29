using BudgetStudio.ViewModels;
using MongoDB.Driver;

namespace BudgetStudio.Infrastructure.Persistence
{
	public class BudgetNoSqlContext : IBudgetNoSqlContext
	{
		private readonly IMongoDatabase _db;

		public BudgetNoSqlContext(IBudgetNoSqlSettings budgetNoSqlSettings)
		{
			if (budgetNoSqlSettings.Enabled == false)
			{
				return;
			}

			var client = new MongoClient(budgetNoSqlSettings.ConnectionString);
			_db = client.GetDatabase(budgetNoSqlSettings.DatabaseName);
		}

		public IMongoCollection<BudgetViewModel> Budgets => _db.GetCollection<BudgetViewModel>("Budgets");
	}
}