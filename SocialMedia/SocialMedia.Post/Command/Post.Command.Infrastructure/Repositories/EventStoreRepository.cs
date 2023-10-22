using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Post.Command.Infrastructure.Config;
using SocialMedia.Core.Events;
using SocialMedia.Core.Infrastructure;

namespace Post.Command.Infrastructure.Repositories;

public class EventStoreRepository : IEventStoreRepository
{
    private readonly IMongoCollection<EventModel> _eventStoreCollection;
    
    public EventStoreRepository(IOptions<MongoDbConfig> config)
    {
        var mongoClient = new MongoClient(config.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(config.Value.Database);

        _eventStoreCollection = mongoDatabase.GetCollection<EventModel>(config.Value.Collection);
    }

    
    public async Task SaveAsync(EventModel @event)
    {
        await _eventStoreCollection.InsertOneAsync(@event).ConfigureAwait(false);
    }

    public async Task<List<EventModel>> FindByAggregateId(Guid aggregateId)
    {
        return await _eventStoreCollection
            .Find(x => x.AggregateIdentifier == aggregateId)
            .ToListAsync()
            .ConfigureAwait(false);
    }
}