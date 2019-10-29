using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DbUp;
using DbUp.Builder;
using Employees.Api.Infrastructure.Persistence;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Employees.Api
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
	}
}
