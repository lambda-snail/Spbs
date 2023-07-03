using AutoMapper;
using Spbs.Ui.Data.Messaging.Events;
using Spbs.Ui.Data.Messaging.Messages;

namespace Spbs.Ui.Features.Expenses.Mapping;

public class ExpenseCreatedEventProfile : Profile
{
    public ExpenseCreatedEventProfile()
    {
        CreateMap<CreateExpenseCommandPayload, Expense>()
            .ForMember(e => e.Items, opt => opt.Ignore())
            .ForMember(e => e.Tags, opt => opt.Ignore())
            .ForMember(e => e.CreatedOn, opt => opt.Ignore())
            .ForMember(e => e.ModifiedOn, opt => opt.Ignore());
    }
}