using SocialMedia.Core.Domain;

namespace SocialMedia.Core.Handlers;

public interface IEventSourcingHandler<T>
{
    Task SaveAsync(AggregateRoot aggregate);
    Task<T> GetByIdAsync(Guid aggregateId);
}