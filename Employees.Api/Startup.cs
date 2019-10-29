using AutoMapper;
using Employees.Api.Application;
using Employees.Api.Infrastructure.Persistence;
using Employees.Api.MapperProfiles;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Employees.Api
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
			AddDbContexts(services);

			services.AddAutoMapper(typeof(EmployeeMapperProfile));

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			services.AddTransient<IEmployeeService, EmployeeService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseMvc();
		}

		private void AddDbContexts(IServiceCollection services)
		{
			var useDb = Configuration["ConnectionStrings:Use"];
			if (useDb == "SqlServer")
			{
				services
					.AddDbContext<EmployeesContext>(
						options =>
						{
							options.UseLazyLoadingProxies();
							options.UseSqlServer(Configuration["ConnectionStrings:SqlServer"]);
							options.EnableDetailedErrors();
						});
			}
			else if (useDb == "PostgreSql")
			{
				services
					.AddEntityFrameworkNpgsql()
					.AddDbContext<EmployeesContext>(
						options =>
						{
							options.UseLazyLoadingProxies();
							options.UseNpgsql(Configuration["ConnectionStrings:PostgreSql"]);
							options.EnableDetailedErrors();
						});
			}
			else
			{
				services.AddDbContext<EmployeesContext>(opt =>
					opt.UseInMemoryDatabase("employees"));
			}
		}
	}
}
