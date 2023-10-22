using SocialMedia.Core.Events;

namespace SocialMedia.Core.Producers;

public interface IEventProducer
{
    Task ProduceAsync(string topic, BaseEvent @event);
}