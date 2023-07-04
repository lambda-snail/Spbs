using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Utilities;
using Spbs.Shared.Data;
using Spbs.Data.Cosmos;
using Spbs.Ui.Data.Messaging.Commands;
using Spbs.Ui.Features.RecurringExpenses.Messaging;

namespace Spbs.Ui.Features.RecurringExpenses;

public class RecurringExpenseWriter: CosmosRepositoryBase<RecurringExpense>, IRecurringExpenseWriterRepository
{
    private readonly RecurringExpenses_CreateExpenseCommandPublisher _publisher;
    private readonly IMapper _mapper;
    private readonly IDateTimeProvider _dateTime;

    public RecurringExpenseWriter(
        CosmosClient client,
        RecurringExpenses_CreateExpenseCommandPublisher publisher,
        IMapper mapper,
        IDateTimeProvider dateTime,
        IOptions<DataConfigurationOptions> options, 
        ILogger<RecurringExpenseWriter> logger) : base(client, options, CosmosTypeConstants.SpbsRecurringExpenses, logger)
    {
        _publisher = publisher;
        _mapper = mapper;
        _dateTime = dateTime;
    }
    
    public async Task<RecurringExpense?> UpsertExpenseAsync(RecurringExpense expense)
    {
        bool isCreate = expense.Id == default;
        var upserted = await Upsert(expense);

        if (isCreate && upserted is not null)
        {
            DateTime now = _dateTime.Now();
            
            var expensePayload = _mapper.Map<CreateExpenseCommandPayload>(upserted);
            expensePayload.Date = upserted.GetNextBillingDate(now);
            await _publisher.ScheduleMessage(
                new CreateExpenseCommand() { Expense = expensePayload },
                expense.GetNextBillingDate(now));
        }

        return upserted;
    }
}