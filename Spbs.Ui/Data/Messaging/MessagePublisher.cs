using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace Spbs.Ui.Data.Messaging;

public class MessagePublisher<T> : IAsyncDisposable
{
    protected readonly ServiceBusSender _serviceBusSender;

#pragma warning disable CS8618
    /// <summary>
    /// ctor for mocking
    /// </summary>
    protected MessagePublisher() { }
#pragma warning restore CS8618
    
    public MessagePublisher(ServiceBusClient client, string queueName)
    {
        ArgumentException.ThrowIfNullOrEmpty(queueName);
        _serviceBusSender = client.CreateSender(queueName);
    }
    
    public virtual Task PublishMessage(T payload)
    {
        var message = CreateMessage(payload);
        return _serviceBusSender.SendMessageAsync(message);
    }

    public virtual Task ScheduleMessage(T payload, DateTime deliveryTime)
    {
        var message = CreateMessage(payload);
        return _serviceBusSender.ScheduleMessageAsync(message, deliveryTime.ToUniversalTime());
    }

    protected virtual ServiceBusMessage CreateMessage(T payload) => new ServiceBusMessage( Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload)) );
    
    public ValueTask DisposeAsync()
    {
        return _serviceBusSender.DisposeAsync();
    }
}