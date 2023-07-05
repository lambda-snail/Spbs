using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using Spbs.Ui.Data.Messaging;
using Spbs.Ui.Data.Messaging.Commands;

namespace Spbs.Ui.Features.RecurringExpenses.Messaging;

/// <summary>
/// Publish create expense commands with message id based on recurring expense id and date.
/// </summary>
public class RecurringExpenses_CreateExpenseCommandPublisher : MessagePublisher<CreateExpenseCommand>
{
    public RecurringExpenses_CreateExpenseCommandPublisher(ServiceBusClient client, IOptions<MessagingOptions> options) :
        base(client, options.Value.ExpensesQueue)
    {}
    
    /// <summary>
    /// ctor for mocking
    /// </summary>
    protected RecurringExpenses_CreateExpenseCommandPublisher() { }

    public override Task PublishMessage(CreateExpenseCommand payload)
    {
        return PublishMessage(payload, RecurringExpenseMessagingConstants.CreateSingleExpense);
    }
    
    public Task PublishMessage(CreateExpenseCommand payload, string originatingSource)
    {
        payload.OriginatingSource = originatingSource;
        return base.PublishMessage(payload);
    }

    public override Task ScheduleMessage(CreateExpenseCommand payload, DateTime deliveryTime)
    {
        return ScheduleMessage(payload, deliveryTime, RecurringExpenseMessagingConstants.ScheduleExpenseCreationCycle);
    }

    public Task ScheduleMessage(CreateExpenseCommand payload, DateTime deliveryTime, string originatingSource)
    {
        payload.OriginatingSource = originatingSource;
        return base.ScheduleMessage(payload, deliveryTime);
    }
    
    protected override ServiceBusMessage CreateMessage(CreateExpenseCommand payload)
    {
        var message = base.CreateMessage(payload);
        message.MessageId = payload.Expense.RecurringExpenseId.ToString() + "-" +
                            payload.Expense.Date.ToString("yyyy-MM-dd");
        return message;
    }
}