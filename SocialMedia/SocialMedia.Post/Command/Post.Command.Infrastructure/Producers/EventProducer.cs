using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using SocialMedia.Core.Events;
using SocialMedia.Core.Producers;

namespace Post.Command.Infrastructure.Producers;

public class EventProducer : IEventProducer
{
    private readonly ProducerConfig _producerConfig;

    public EventProducer(IOptions<ProducerConfig> options)
    {
        _producerConfig = options.Value;
    }
    
    public async Task ProduceAsync(string topic, BaseEvent @event)
    {
        using var producer = new ProducerBuilder<string, string>(_producerConfig)
            .SetKeySerializer(Serializers.Utf8)
            .SetValueSerializer(Serializers.Utf8)
            .Build();

        var eventMessage = new Message<string, string>()
        {
            Key = Guid.NewGuid().ToString(),
            Value = JsonSerializer.Serialize(@event, @event.GetType())
        };
        
        var deliveryResult = await producer.ProduceAsync(topic, eventMessage);

        if (deliveryResult.Status == PersistenceStatus.NotPersisted)
        {
            throw new Exception($"Could not produce {@event.GetType().Name} message to topic - {topic} due to the following reason: {deliveryResult.Message}.");
        }
    }
}