using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Utilities;
using Spbs.Ui.Data.Messaging;
using Spbs.Ui.Data.Messaging.Commands;
using Spbs.Ui.Data.Messaging.Events;
using Spbs.Ui.Features.Expenses;

namespace Spbs.Ui.Features.RecurringExpenses.Messaging;

public class CreateExpenseCommandConsumer : BackgroundService, IAsyncDisposable
{
    private readonly IExpenseWriterRepository _expenseWriter;
    private readonly MessagePublisher<ExpenseCreatedForRecurringEvent> _publisher;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateExpenseCommandConsumer> _logger;

    //private readonly ServiceBusProcessor _processor;
    private readonly ServiceBusReceiver _receiver;

    public CreateExpenseCommandConsumer(
        ServiceBusClient client, 
        IExpenseWriterRepository expenseWriter,
        ExpenseCreatedForRecurringEventPublisher publisher, 
        IMapper mapper, 
        IOptions<MessagingOptions> options,
        ILogger<CreateExpenseCommandConsumer> logger)
    {
        _expenseWriter = expenseWriter;
        _publisher = publisher;
        _mapper = mapper;
        _logger = logger;
        _receiver = client.CreateReceiver(options.Value.ExpensesQueue);
        //_processor = client.CreateProcessor(options.RecurringExpensesQueue);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var messageProcessed = await ProcessMessage(stoppingToken);
            if (!messageProcessed)
            {
                await Task.Delay(10000, stoppingToken);
            }
        }
    }

    private async Task<bool> ProcessMessage(CancellationToken stoppingToken)
    {
        ServiceBusReceivedMessage receivedMessage =
            await _receiver.ReceiveMessageAsync(TimeSpan.FromSeconds(2), cancellationToken: stoppingToken);

        if (receivedMessage == null)
        {
            return false;
        }
        
        _logger.LogTrace("{LoggingEntity}: Received message from queue {MessageId}", nameof(CreateExpenseCommandConsumer), receivedMessage.MessageId);

        if (!receivedMessage.Body.TryParseAsOBject<CreateExpenseCommand>(out var createExpenseCommand) ||
            createExpenseCommand?.Expense is null)
        {
            _logger.LogTrace("{LoggingEntity}: Unable to parse {MessageId} or command body empty - abandoning message", nameof(CreateExpenseCommandConsumer), receivedMessage.MessageId);
            await _receiver.AbandonMessageAsync(receivedMessage, cancellationToken: stoppingToken);
            return false;
        }

        var expensePayload = createExpenseCommand.Expense;
        var expense = _mapper.Map<Expense>(expensePayload);

        if (expense.UserId == Guid.Empty)
        {
            _logger.LogError("{LoggingEntity}: Received command {MessageId} to create expense but user id was empty",
                nameof(CreateExpenseCommandConsumer), receivedMessage.MessageId);
            await _receiver.DeadLetterMessageAsync(receivedMessage, cancellationToken: stoppingToken);
            return false;
        }

        expense.Id = Guid.NewGuid();
        await _expenseWriter.InsertExpenseAsync(expense);

        if (expense.Recurring)
        {
            await _publisher.PublishMessage(new()
            {
                ExpenseId = expense.Id,
                RecurringExpenseId = expense.RecurringExpenseId,
                UserId = expense.UserId,
                Date = expense.Date,
                Total = expense.Total
            });
        }

        await _receiver.CompleteMessageAsync(receivedMessage, stoppingToken);
        return true;
    }

    public ValueTask DisposeAsync()
    {
        //await _processor.StopProcessingAsync();
        //await _processor.DisposeAsync();
        return _receiver.DisposeAsync();
    }
}