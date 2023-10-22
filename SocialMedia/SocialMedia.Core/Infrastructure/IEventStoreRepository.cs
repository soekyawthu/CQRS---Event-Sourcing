using SocialMedia.Core.Events;

namespace SocialMedia.Core.Infrastructure;

public interface IEventStoreRepository
{
    Task SaveAsync(EventModel @event);
    Task<List<EventModel>> FindByAggregateId(Guid aggregateId);
}