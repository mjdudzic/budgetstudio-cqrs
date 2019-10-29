using System;
using BudgetStudio.Domain.AggregatesModel.BudgetAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BudgetStudio.Infrastructure.Persistence
{
	public class ExtraCostConfiguration : IEntityTypeConfiguration<ExtraCost>
	{
		public void Configure(EntityTypeBuilder<ExtraCost> builder)
		{
			builder.ToTable("extra_costs");

			builder.Property<Guid>("Id")
				.HasColumnName("id")
				.IsRequired();
			builder.HasKey("Id");

			builder.Property<Guid>("BudgetId")
				.HasColumnName("budget_id")
				.IsRequired();

			builder.Property(i => i.Description)
				.HasColumnName("description")
				.IsRequired();

			builder.OwnsOne(i => i.Cost)
				.Property(i => i.Amount).HasColumnName("cost_amount");

			builder.OwnsOne(i => i.Cost)
				.Property(i => i.Currency).HasColumnName("cost_currency");
		}
	}
}