namespace BudgetStudio.Infrastructure.Persistence
{
	public class BudgetNoSqlSettings : IBudgetNoSqlSettings
	{
		public bool Enabled { get; set; }
		public string ConnectionString { get; set; }
		public string DatabaseName { get; set; }
	}
}