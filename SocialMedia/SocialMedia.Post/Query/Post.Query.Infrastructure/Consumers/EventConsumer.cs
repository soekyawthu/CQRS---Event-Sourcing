using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Post.Query.Infrastructure.Converters;
using Post.Query.Infrastructure.Handlers;
using SocialMedia.Core.Consumers;
using SocialMedia.Core.Events;

namespace Post.Query.Infrastructure.Consumers;

public class EventConsumer : IEventConsumer
{
    private readonly IEventHandler _eventHandler;
    private readonly ConsumerConfig _consumerConfig;
    
    public EventConsumer(IOptions<ConsumerConfig> options, IEventHandler eventHandler)
    {
        _eventHandler = eventHandler;
        _consumerConfig = options.Value;
    }
    
    public void Consume(string topic)
    {
        var consumer = new ConsumerBuilder<string, string>(_consumerConfig)
            .SetKeyDeserializer(Deserializers.Utf8)
            .SetValueDeserializer(Deserializers.Utf8)
            .Build();
        
        consumer.Subscribe(topic);

        while (true)
        {
            var consumeResult = consumer.Consume();
            if (consumeResult is null) continue;
            
            var options = new JsonSerializerOptions { Converters = { new EventJsonConverter() } };
            var @event = JsonSerializer.Deserialize<BaseEvent>(consumeResult.Message.Value, options);

            if (@event is null)
            {
                throw new ArgumentNullException(nameof(@event), "event is null!");
            }

            var handlerMethod = _eventHandler.GetType().GetMethod("On", new[]{ @event.GetType() });

            if (handlerMethod is null)
            {
                throw new ArgumentNullException(nameof(handlerMethod), "Could not find event handler method!");
            }
            
            handlerMethod.Invoke(_eventHandler, new object?[] { @event });
        }
    }
}