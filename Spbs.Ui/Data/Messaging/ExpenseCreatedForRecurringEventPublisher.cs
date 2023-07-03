using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using Spbs.Ui.Data.Messaging.Events;

namespace Spbs.Ui.Data.Messaging;

/// <summary>
/// Publisher that notifies about expenses created for recurring events on the recurring expenses queue
/// </summary>
public class ExpenseCreatedForRecurringEventPublisher : MessagePublisher<ExpenseCreatedForRecurringEvent>
{
    public ExpenseCreatedForRecurringEventPublisher(ServiceBusClient client, IOptions<MessagingOptions> options) :
        base(client, options.Value.RecurringExpensesQueue)
    {
        
    }
}