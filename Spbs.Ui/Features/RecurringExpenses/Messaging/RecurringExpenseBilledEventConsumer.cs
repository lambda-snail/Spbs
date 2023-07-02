using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Hosting;
using Spbs.Ui.Data.Messaging;

namespace Spbs.Ui.Features.RecurringExpenses.Messaging;

public class RecurringExpenseBilledEventConsumer : BackgroundService, IAsyncDisposable
{
    private readonly ServiceBusProcessor _processor;
    
    /// <summary>
    /// ctor for mocking
    /// </summary>
#pragma warning disable CS8618
    protected RecurringExpenseBilledEventConsumer() { }
#pragma warning restore CS8618
    
    public RecurringExpenseBilledEventConsumer(ServiceBusClient client, MessagingOptions options)
    {
        //client.CreateReceiver()
        _processor = client.CreateProcessor(options.RecurringExpensesQueue);
    }
    
    private async Task MessageHandler(ProcessMessageEventArgs args)
    {
        string body = args.Message.Body.ToString();

        await args.CompleteMessageAsync(args.Message);
    }
    
    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.ToString());
        return Task.CompletedTask;
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _processor.ProcessMessageAsync += MessageHandler;
        _processor.ProcessErrorAsync += ErrorHandler;

        if (!_processor.IsProcessing)
        {
            Console.WriteLine("Message Processor up and running!");
            return _processor.StartProcessingAsync(stoppingToken);   
        }

        return Task.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        await _processor.StopProcessingAsync();
        await _processor.DisposeAsync();
    }
}