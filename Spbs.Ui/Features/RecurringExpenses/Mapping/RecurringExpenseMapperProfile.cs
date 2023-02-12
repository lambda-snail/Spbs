using AutoMapper;

namespace Spbs.Ui.Features.RecurringExpenses.Mapping;

public class RecurringExpenseMapperProfile : Profile
{
    public RecurringExpenseMapperProfile()
    {
        CreateMap<EditRecurringExpenseViewModel, RecurringExpense>();
        CreateMap<RecurringExpense, EditRecurringExpenseViewModel>();
    }
}