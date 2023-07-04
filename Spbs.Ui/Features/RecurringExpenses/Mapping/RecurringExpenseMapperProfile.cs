using AutoMapper;
using Spbs.Ui.Data.Messaging.Commands;

namespace Spbs.Ui.Features.RecurringExpenses.Mapping;

public class RecurringExpenseMapperProfile : Profile
{
    public RecurringExpenseMapperProfile()
    {
        CreateMap<EditRecurringExpenseViewModel, RecurringExpense>();
        CreateMap<RecurringExpense, EditRecurringExpenseViewModel>();

        CreateMap<RecurringExpense, CreateExpenseCommandPayload>()
            .ForMember(cmd => cmd.RecurringExpenseId, opt => opt.MapFrom(e => e.Id))
            .ForMember(cmd => cmd.Recurring, opt => opt.MapFrom(e => true))
            .ForMember(cmd => cmd.Date, opt => opt.Ignore())
            .ForMember(cmd => cmd.Venue, opt => opt.MapFrom(e => e.BillingPrincipal));
    }
}