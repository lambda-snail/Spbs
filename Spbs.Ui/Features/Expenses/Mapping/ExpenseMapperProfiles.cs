using AutoMapper;

namespace Spbs.Ui.Features.Expenses.Mapping;

public class ExpenseMapperProfiles : Profile
{
    public ExpenseMapperProfiles()
    {
        CreateMap<EditExpenseViewModel, Expense>();
        CreateMap<Expense, EditExpenseViewModel>();
        
        CreateMap<EditExpenseItemViewModel, ExpenseItem>();
        CreateMap<ExpenseItem, EditExpenseItemViewModel>();
        
        CreateMap<ExpenseItem, ExpenseItem>();
    }
}