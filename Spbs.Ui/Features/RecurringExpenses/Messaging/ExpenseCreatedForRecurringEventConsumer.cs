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

namespace Spbs.Ui.Features.RecurringExpenses.Messaging;

public class ExpenseCreatedForRecurringEventConsumer : BackgroundService, IAsyncDisposable
{
    private readonly IRecurringExpenseWriterRepository _expenseWriter;
    private readonly RecurringExpenses_CreateExpenseCommandPublisher _publisher;
    private readonly IMapper _mapper;
    private readonly IRecurringExpenseReaderRepository _expenseReader;
    private readonly ILogger<ExpenseCreatedForRecurringEventConsumer> _logger;
    private readonly ServiceBusReceiver _receiver;

    public ExpenseCreatedForRecurringEventConsumer(
        ServiceBusClient client,
        IRecurringExpenseReaderRepository expenseReader,
        IRecurringExpenseWriterRepository expenseWriter,
        RecurringExpenses_CreateExpenseCommandPublisher publisher,
        IMapper mapper,
        IOptions<MessagingOptions> options,
        ILogger<ExpenseCreatedForRecurringEventConsumer> logger)
    {
        _expenseReader = expenseReader;
        _expenseWriter = expenseWriter;
        _publisher = publisher;
        _mapper = mapper;
        _logger = logger;
        _receiver = client.CreateReceiver(options.Value.RecurringExpensesQueue);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var messageProcessed = await ProcessMessage(stoppingToken);
            if (!messageProcessed)
            {
                await Task.Delay(1000 * 60 * 8, stoppingToken);
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

        _logger.LogTrace("{LoggingEntity}: Received message from queue {MessageId}",
            nameof(ExpenseCreatedForRecurringEventConsumer), receivedMessage.MessageId);

        if (!receivedMessage.Body.TryParseAsOBject<ExpenseCreatedForRecurringEvent>(out var expenseCreatedEvent) ||
            expenseCreatedEvent is null)
        {
            _logger.LogTrace("{LoggingEntity}: Unable to parse {MessageId} or body empty - abandoning message",
                nameof(CreateExpenseCommandConsumer), receivedMessage.MessageId);
            await _receiver.AbandonMessageAsync(receivedMessage, cancellationToken: stoppingToken);
            return false;
        }

        var expense = await _expenseReader.GetByIdAsync(expenseCreatedEvent.Expense.UserId,
            expenseCreatedEvent.Expense.RecurringExpenseId);
        if (expense is null)
        {
            _logger.LogWarning(
                "{LoggingEntity}: Attempted to update recurring expense {RecurringExpenseId} for user {UserId} but expense was not found",
                nameof(ExpenseCreatedForRecurringEventConsumer), expenseCreatedEvent.Expense.RecurringExpenseId,
                expenseCreatedEvent.Expense.UserId);
            await _receiver.DeadLetterMessageAsync(receivedMessage, cancellationToken: stoppingToken);
            return false;
        }

        if (expense.PaymentHistory is null or { Count: 0 })
        {
            expense.PaymentHistory = new();
        }

        expense.PaymentHistory.Add(new()
        {
            Id = Guid.NewGuid(),
            Date = expenseCreatedEvent.Expense.Date,
            ExpenseId = expenseCreatedEvent.Expense.ExpenseId,
            Total = expenseCreatedEvent.Expense.Total
        });

        await _expenseWriter.UpsertExpenseAsync(expense);

        if (expenseCreatedEvent.OriginatingSource == RecurringExpenseMessagingConstants.ScheduleExpenseCreationCycle)
        {
            var commandPayload = _mapper.Map<CreateExpenseCommandPayload>(expense);
            commandPayload.Recurring = true;

            var nextPayment = expense.GetBillingDateNextMonth(expenseCreatedEvent.Expense.Date);
            commandPayload.Date = nextPayment;

            await _publisher.ScheduleMessage(new CreateExpenseCommand()
                {
                    OriginatingSource = RecurringExpenseMessagingConstants.ScheduleExpenseCreationCycle,
                    Expense = commandPayload
                },
                nextPayment);
        }

        await _receiver.CompleteMessageAsync(receivedMessage, cancellationToken: stoppingToken);
        _logger.LogTrace(
            "{LoggingEntity}: Updated recurring expense {RecurringExpenseId} and completed message {MessageId}",
            nameof(ExpenseCreatedForRecurringEventConsumer), expenseCreatedEvent.Expense.RecurringExpenseId,
            receivedMessage.MessageId);

        return true;
    }

    public ValueTask DisposeAsync()
    {
        return _receiver.DisposeAsync();
    }
}