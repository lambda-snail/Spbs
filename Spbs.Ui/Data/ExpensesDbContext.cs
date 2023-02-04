using System;
using Microsoft.EntityFrameworkCore;
using Spbs.Ui.Features.Expenses;
using Spbs.Ui.Features.Users;

namespace Spbs.Ui.Data;

public class ExpensesDbContext : DbContext
{
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<ExpenseItem> ExpenseItems { get; set; }

    public ExpensesDbContext(DbContextOptions<ExpensesDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Expense>(builder =>
        {
            builder.Property(e => e.Description).HasColumnType("nvarchar(2048)");
            builder.Property(e => e.Name).HasColumnType("nvarchar(128)");
            builder.Property(e => e.Tags).HasColumnType("nvarchar(256)")
                .IsRequired(false); // TODO: Should be own table?
            builder.Property(e => e.Venue).HasColumnType("nvarchar(256)"); // TODO: Should be own table!
            builder.Property(i => i.Currency).HasColumnType("nvarchar(8)"); // TODO: How to deal with currency?
            
            builder.HasIndex(e => e.Venue);

            builder.Property(e => e.Name).IsRequired();
            builder.Property(e => e.Recurring).HasDefaultValue(false);
            builder.Property(e => e.OwningUserId).IsRequired();

            builder.HasOne<User>().WithMany().HasPrincipalKey(u => u.AzureAdId).HasForeignKey(e => e.OwningUserId);
        });

        modelBuilder.Entity<Expense>().Ignore(x => x.Total);

        modelBuilder.Entity<ExpenseItem>(builder =>
        {
            builder.Property(i => i.Name).HasColumnType("nvarchar(128)");

            builder.Property(i => i.Name).IsRequired();
            builder.Property(i => i.Quantity).IsRequired();
            builder.Property(i => i.Price).IsRequired();
        });
    }
}