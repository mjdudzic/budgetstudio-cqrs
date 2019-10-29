using System.Reflection;
using AutoMapper;
using BudgetStudio.Api.Application.Queries;
using BudgetStudio.Api.Filters;
using BudgetStudio.Api.Hubs;
using BudgetStudio.Api.MapperProfiles;
using BudgetStudio.Domain.AggregatesModel.BudgetAggregate;
using BudgetStudio.Infrastructure.Persistence;
using BudgetStudio.Infrastructure.Repositories;
using BudgetStudio.Infrastructure.Services;
using BudgetStudio.ViewModels;
using Hangfire;
using Hangfire.Console;
using Hangfire.MemoryStorage;
using Hangfire.PostgreSql;
using MediatR;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BudgetStudio.Api
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			AddConfiguration(services);
			AddDbContexts(services);
			AddHangfire(services);

			services.AddMvcCore(options =>
			{
				options.EnableEndpointRouting = false;
			});

			services.AddMemoryCache();

			services.AddHttpClient();

			services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);

			services.AddOData();
			services.AddTransient<BudgetsODataModelBuilder>();

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			services.AddAutoMapper(typeof(BudgetMapperProfile));

			services.AddSignalR();

			services.AddTransient<IBudgetRepository, BudgetRepository>();
			services.AddTransient<IBudgetNoSqlContext, BudgetNoSqlContext>();
			services.AddTransient<IEmployeeCostService, EmployeeCostService>();
			services.AddTransient<ICommissionCalculationService, CommissionCalculationService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, BudgetsODataModelBuilder budgetsODataModelBuilder)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			AddHangfireMiddleware(app);

			app.UseCors(builder =>
			{
				builder.WithOrigins("http://localhost:5001")
					.AllowAnyHeader()
					.AllowAnyMethod()
					.AllowCredentials();
			});

			app.UseSignalR(routes =>
			{
				routes.MapHub<NotificationHub>("/notification");
			});

			app.UseMvc(builder =>
			{
				builder.Select().Expand().Filter().OrderBy().Count().MaxTop(100);
				builder.MapODataServiceRoute(
					"odata", 
					"odata", 
					budgetsODataModelBuilder.GetEdmModel(app.ApplicationServices));
			});
		}

		private void AddConfiguration(IServiceCollection services)
		{
			var budgetNoSqlSettings = new BudgetNoSqlSettings();
			Configuration.Bind("MongoDb", budgetNoSqlSettings);
			services.AddSingleton<IBudgetNoSqlSettings>(budgetNoSqlSettings);
		}

		private void AddDbContexts(IServiceCollection services)
		{
			var useDb = Configuration["ConnectionStrings:Use"];
			if (useDb == "SqlServer")
			{
				services
					.AddDbContext<BudgetContext>(
						options =>
						{
							options.UseSqlServer(Configuration["ConnectionStrings:SqlServer"]);
							options.EnableDetailedErrors();
						});
			}
			else if (useDb == "PostgreSql")
			{
				services
					.AddEntityFrameworkNpgsql()
					.AddDbContext<BudgetContext>(
						options =>
						{
							options.UseNpgsql(Configuration["ConnectionStrings:PostgreSql"]);
							options.EnableDetailedErrors();
						});
			}
			else
			{
				services.AddDbContext<BudgetContext>(opt =>
					opt.UseInMemoryDatabase("budget_studio"));
			}
		}

		private void AddHangfire(IServiceCollection services)
		{
			services.AddHangfire(config =>
			{
				var useDb = Configuration["ConnectionStrings:Use"];
				if (useDb == "SqlServer")
				{
					config.UseSqlServerStorage(Configuration["ConnectionStrings:SqlServer"]);
				}
				else if (useDb == "PostgreSql")
				{
					config.UsePostgreSqlStorage(Configuration["ConnectionStrings:PostgreSql"]);
				}
				else
				{
					config.UseMemoryStorage();
				}

				config.UseConsole();
			});
		}

		private void AddHangfireMiddleware(IApplicationBuilder app)
		{
			app.UseHangfireServer();

			app.UseHangfireDashboard("/dashboard", new DashboardOptions
			{
				Authorization = new[] { new FakeDashboardAuthFilter() }
			});
		}
	}
}
