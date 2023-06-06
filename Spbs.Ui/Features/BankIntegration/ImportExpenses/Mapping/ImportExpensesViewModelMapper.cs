using System;
using System.Collections.Generic;
using AutoMapper;
using Integrations.Nordigen.Models;
using Spbs.Ui.Features.Expenses;

namespace Spbs.Ui.Features.BankIntegration.ImportExpenses.Mapping;

public class ImportExpensesViewModelMapper : Profile
{
    public ImportExpensesViewModelMapper()
    {
        CreateMap<NordigenTransaction, ImportExpensesViewModel>()
            .ForMember(vm => vm.DebtorAccount, opt => opt.MapFrom(src => src.DebtorAccount))
            
            .ForMember(vm => vm.IncludeInImport, opt => opt.Ignore())
            .ForMember(vm => vm.IsPending, opt => opt.Ignore());

        CreateMap<DebtorAccount, DebtorAccountViewModel>();
        CreateMap<TransactionAmount, TransactionAmountViewModel>();

        CreateMap<ImportExpensesViewModel, Expense>()
            .ForMember(e => e.Currency, opt => opt.MapFrom(vm => vm.TransactionAmount.Currency))
            .ForMember(e => e.Date, opt => opt.MapFrom(vm => vm.ValueDate.ToDateTime(TimeOnly.MinValue)))
            .ForMember(e => e.Venue, opt => opt.MapFrom(vm => vm.RemittanceInformationUnstructured))
            .ForMember(e => e.Description, opt => opt.MapFrom(vm => "Expense imported with id " + vm.TransactionId))
            .ForMember(e => e.Name, opt => opt.MapFrom(vm => vm.RemittanceInformationUnstructured))
            
            // Untill we can support expenses with a total but no expense items.
            .ForMember(e => e.Items, opt => 
                opt.MapFrom(vm =>
                    new List<ExpenseItem>
                    {
                        new ExpenseItem
                        {
                            Name = vm.RemittanceInformationUnstructured,
                            Price = vm.TransactionAmount.Amount,
                            Quantity = 1
                        }
                    }))
            .ForMember(e => e.Total, opt => opt.Ignore())
            .ForMember(e => e._total, opt => opt.Ignore())
            
            .ForMember(e => e.Id, opt => opt.Ignore())
            .ForMember(e => e.Recurring, opt => opt.Ignore())
            .ForMember(e => e.Tags, opt => opt.Ignore())
            .ForMember(e => e.CreatedOn, opt => opt.Ignore())
            .ForMember(e => e.ModifiedOn, opt => opt.Ignore())
            .ForMember(e => e.UserId, opt => opt.Ignore());
    }
}