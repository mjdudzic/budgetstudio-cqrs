using System;
using BudgetStudio.Domain.AggregatesModel.BudgetAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BudgetStudio.Infrastructure.Persistence
{
	public class EmployeeCostConfiguration : IEntityTypeConfiguration<EmployeeCost>
	{
		public void Configure(EntityTypeBuilder<EmployeeCost> builder)
		{
			builder.ToTable("employee_costs");

			builder.Property<Guid>("Id")
				.HasColumnName("id")
				.IsRequired();
			builder.HasKey("Id");

			builder.Property<Guid>("BudgetId")
				.HasColumnName("budget_id")
				.IsRequired();

			builder.Property(i => i.EmployeeCode)
				.HasColumnName("employee_code")
				.IsRequired();

			builder.OwnsOne(i => i.Participation)
				.Property(i => i.StartedAt).HasColumnName("participation_started_at");

			builder.OwnsOne(i => i.Participation)
				.Property(i => i.EndedAt).HasColumnName("participation_ended_at");

			builder.OwnsOne(i => i.Cost)
				.Property(i => i.Amount).HasColumnName("cost_amount");

			builder.OwnsOne(i => i.Cost)
				.Property(i => i.Currency).HasColumnName("cost_currency");
		}
	}
}