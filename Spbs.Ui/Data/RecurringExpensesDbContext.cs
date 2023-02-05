using Microsoft.EntityFrameworkCore;
using Spbs.Ui.Features.RecurringExpenses;
using Spbs.Ui.Features.Users;

namespace Spbs.Ui.Data;

public class RecurringExpensesDbContext : DbContext
{
    public DbSet<RecurringExpense> RecurringExpenses { get; set; }

    public RecurringExpensesDbContext(DbContextOptions<RecurringExpensesDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RecurringExpense>(builder =>
        {
            builder.Property(re => re.Name).HasColumnType("nvarchar(128)");
            builder.Property(re => re.BillingPrincipal).HasColumnType("nvarchar(128)");
            builder.Property(re => re.Description).HasColumnType("nvarchar(2048)");
            builder.Property(re => re.Currency).HasColumnType("nvarchar(8)");
            builder.Property(re => re.Tags).HasColumnType("nvarchar(256)").IsRequired(false);

            builder.Property(re => re.Name).IsRequired();
            builder.Property(re => re.Total).IsRequired();
            builder.Property(re => re.BillingDate).IsRequired();
            builder.Property(re => re.BillingPrincipal).IsRequired();
            builder.Property(re => re.BillingType).IsRequired();
            builder.Property(re => re.RecurrenceType).IsRequired();
            
            builder.HasIndex(re => re.Name);
            builder.HasIndex(re => re.BillingDate);
            builder.HasIndex(re => re.BillingType);
            builder.HasOne<User>().WithMany().HasPrincipalKey(u => u.AzureAdId).HasForeignKey(e => e.OwningUserId);
        });

        modelBuilder.Entity<RecurringExpenseHistoryItem>(builder =>
        {
            builder.Property(re => re.Total).IsRequired();
            builder.Property(re => re.Date).IsRequired();
        });

        modelBuilder.Entity<User>().ToTable(nameof(User), t => t.ExcludeFromMigrations());
    }
}