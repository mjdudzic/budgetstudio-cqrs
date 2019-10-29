using System.Threading;
using System.Threading.Tasks;
using BudgetStudio.Domain.AggregatesModel.BudgetAggregate;
using BudgetStudio.Infrastructure.Extensions;
using BudgetStudio.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetStudio.Infrastructure.Persistence
{
	public class BudgetContext : DbContext
	{
		private readonly IMediator _mediator;

		public BudgetContext(DbContextOptions options, IMediator mediator)
			: base(options)
		{
			_mediator = mediator;
		}

		public DbSet<Budget> Budgets { get; set; }

		public DbSet<EmployeeCost> EmployeeCosts { get; set; }

		public DbSet<ExtraCost> ExtraCosts { get; set; }

		public DbSet<BudgetOdataViewModel> BudgetViewItems { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new BudgetConfiguration());
			modelBuilder.ApplyConfiguration(new EmployeeCostConfiguration());
			modelBuilder.ApplyConfiguration(new ExtraCostConfiguration());
			modelBuilder.ApplyConfiguration(new BudgetOdataConfiguration());
		}

		public async Task SaveEntitiesAsync(CancellationToken cancellationToken)
		{
			await base.SaveChangesAsync(cancellationToken);

			await _mediator.DispatchDomainEventsAsync(this);
		}
	}
}
