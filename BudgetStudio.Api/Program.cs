using System;
using System.IO;
using System.Linq;
using System.Threading;
using BudgetStudio.Domain.AggregatesModel.BudgetAggregate;
using BudgetStudio.Infrastructure.Persistence;
using BudgetStudio.Infrastructure.Services;
using DbUp;
using DbUp.Builder;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BudgetStudio.Api
{
	public class Program
	{
		private const int ProbesCountMax = 10;

		private static IConfiguration Configuration { get; } = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
			.AddEnvironmentVariables()
			.Build();

		public static void Main(string[] args)
		{
			BuildDatabase();

			var host = CreateWebHostBuilder(args).Build();

			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				services.GetRequiredService<BudgetContext>();

				Initialize(services);
			}

			host.Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>();

		private static void BuildDatabase()
		{
			var useDb = Configuration["ConnectionStrings:Use"];

			if (useDb == "InMemory")
			{
				return;
			}
			
			var connectionStringKey = useDb == "SqlServer"
				? "ConnectionStrings:SqlServer"
				: "ConnectionStrings:PostgreSql";

			var scriptsPath = useDb == "SqlServer"
				? "Database/SqlServer"
				: "Database/PostgreSql";

			var engineBuilderFunc = useDb == "SqlServer"
				? new Func<string, UpgradeEngineBuilder>(s => DeployChanges.To.SqlDatabase(s))
				: new Func<string, UpgradeEngineBuilder>(s => DeployChanges.To.PostgresqlDatabase(s));

			BuildSelectedDatabase(
				connectionStringKey,
				scriptsPath,
				engineBuilderFunc);
		}

		private static void BuildSelectedDatabase(
			string connectionStringKey,
			string scriptsPath,
			Func<string, UpgradeEngineBuilder> engineBuilderFunc)
		{
			var connectionString = Configuration.GetSection(connectionStringKey).Value;

			CheckDbExists(connectionString, 1);

			var result =
				engineBuilderFunc(connectionString)
					.WithScriptsFromFileSystem(Path.Combine(Directory.GetCurrentDirectory(), scriptsPath))
					.LogToConsole()
					.Build()
					.PerformUpgrade();

			if (!result.Successful)
			{
				Console.WriteLine(result.Error);
			}
		}

		private static void CheckDbExists(string connectionString, int probeCount)
		{
			try
			{
				Console.WriteLine($"Probe {probeCount}/{ProbesCountMax}: Checking db connection...");

				var useDb = Configuration["ConnectionStrings:Use"];
				if (useDb == "SqlServer")
				{
					EnsureDatabase.For.SqlDatabase(connectionString);
				}
				else if (useDb == "PostgreSql")
				{
					EnsureDatabase.For.PostgresqlDatabase(connectionString);
				}

				Console.WriteLine($"Probe {probeCount}/{ProbesCountMax}: db connection OK");
			}
			catch (Exception e)
			{
				Console.WriteLine($"Probe {probeCount}/{ProbesCountMax}: Cannot connect to db: {e.Message}");
				if (probeCount == ProbesCountMax)
					throw;

				var waitTime = probeCount * 1000 * 5;

				Console.WriteLine($"Probe {probeCount}/{ProbesCountMax}: Wait {waitTime / 1000} seconds and try again ...");

				Thread.Sleep(waitTime);

				CheckDbExists(connectionString, probeCount + 1);
			}
		}

		private static void Initialize(IServiceProvider serviceProvider)
		{
			using (var context = new BudgetContext(
				serviceProvider.GetRequiredService<DbContextOptions<BudgetContext>>(),
				serviceProvider.GetRequiredService<IMediator>()))
			{
				if (context.Budgets.Any())
				{
					return;
				}

				context.AddRange(GetTestBudget());

				context.SaveChanges();
			}
		}

		private static Budget GetTestBudget()
		{
			var budget = new Budget(
				Guid.NewGuid(),
				Guid.NewGuid());

			budget.AddEmployeesCost(
				new EmployeeIdentity(Guid.NewGuid().ToString("N")),
				new Participation(DateTime.UtcNow, DateTime.UtcNow.AddYears(1)),
				new EmployeeCostService()).GetAwaiter().GetResult();

			budget.AddExtraCost(new ExtraCost
			{
				Description = "DDD, CQRS training",
				Cost = new Price(1000, "MXN")
			});

			return budget;
		}
	}
}
