namespace BudgetStudio.Infrastructure.Persistence
{
	public interface IBudgetNoSqlSettings
	{
		bool Enabled { get; set; }
		string ConnectionString { get; set; }
		string DatabaseName { get; set; }
	}
}