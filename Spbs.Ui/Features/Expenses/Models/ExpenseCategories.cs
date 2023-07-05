using System.Collections.Generic;
using System.Linq;
using MudBlazor;

namespace Spbs.Ui.Features.Expenses.Models;

public class Category
{
    public string Name { get; init; }
    public string MudIcon { get; init; }
}

public class ExpenseCategories
{
    public static readonly Category Housing = new() { Name = "Housing", MudIcon = Icons.Material.Filled.House };
    public static readonly Category Transportation = new() { Name = "Transportation", MudIcon = Icons.Material.Filled.House };
    public static readonly Category Food = new() { Name = "Food", MudIcon = Icons.Material.Filled.LocalDining };
    public static readonly Category DiningOut = new() { Name = "Dining Out", MudIcon = Icons.Material.Filled.Fastfood };
    public static readonly Category Utilities = new() { Name = "Utilities", MudIcon = Icons.Material.Filled.ElectricMeter };
    public static readonly Category Insurance = new() { Name = "Insurance", MudIcon = Icons.Material.Filled.HealthAndSafety };
    public static readonly Category Healthcare = new() { Name = "Healthcare", MudIcon = Icons.Material.Filled.LocalHospital };
    public static readonly Category SavingsInvestment = new() { Name = "Savings & Investment", MudIcon = Icons.Material.Filled.Savings };
    public static readonly Category DebtPayment = new() { Name = "Debt Payment", MudIcon = Icons.Material.Filled.MonetizationOn };
    public static readonly Category PersonalSpending = new() { Name = "Personal Spending", MudIcon = Icons.Material.Filled.SelfImprovement };
    public static readonly Category Entertainment = new() { Name = "Entertainment", MudIcon = Icons.Material.Filled.TheaterComedy };
    public static readonly Category Misc = new() { Name = "Misc", MudIcon = Icons.Material.Filled.Bento };
    public static readonly Category ChildCare = new() { Name = "ChildCare", MudIcon = Icons.Material.Filled.ChildFriendly };
    public static readonly Category Subscriptions = new() { Name = "Subscriptions", MudIcon = Icons.Material.Filled.Subscriptions };
    public static readonly Category Pets = new() { Name = "Pets", MudIcon = Icons.Material.Filled.Pets };
    public static readonly Category Travel = new() { Name = "Travel", MudIcon = Icons.Material.Filled.TravelExplore };
}

public class ExpenseCategoryUtils
{
    private static readonly List<Category> _categories = new()
    {
        ExpenseCategories.Housing,
        ExpenseCategories.Transportation, 
        ExpenseCategories.Food,
        ExpenseCategories.DiningOut,
        ExpenseCategories.Utilities,
        ExpenseCategories.Insurance,
        ExpenseCategories.Healthcare,
        ExpenseCategories.SavingsInvestment,
        ExpenseCategories.DebtPayment,
        ExpenseCategories.PersonalSpending, 
        ExpenseCategories.Entertainment,
        ExpenseCategories.Misc,
        ExpenseCategories.ChildCare,
        ExpenseCategories.Subscriptions,
        ExpenseCategories.Pets,
        ExpenseCategories.Travel
    };

    public static IReadOnlyList<Category> GetAllCategories() => _categories;

    public static string? GetMudIcon(string? category)
    {
        if (category is null)
        {
            return Icons.Material.Filled.QuestionMark;
        }
        
        // Not very efficient, but simple enough since we only have a few categories
        return _categories.SingleOrDefault(c => c.Name == category)?.MudIcon;
    }
}