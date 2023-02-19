using CQRS.Core.Domain;
using CQRS.Core.Events;
using MongoDB.Driver;

namespace Post.Cmd.Infrastructure.Repository;

public class EventStoreRepository : IEventStoreRepository
{
    private readonly IMongoCollection<EventModel> _eventStoreCollection;

    public EventStoreRepository()
    {
        // should be from IOptions<MongoDbOptions>

        var connString = "mongodb://localhost:27017";
        var dataBase = "socialMedia";
        var collection = "eventStore";

        var mongoClient = new MongoClient(connString);
        var mongoDatabase = mongoClient.GetDatabase(dataBase);

        _eventStoreCollection = mongoDatabase.GetCollection<EventModel>(collection);
    }

    public async Task<List<EventModel>> FindAllAsync()
        => await _eventStoreCollection.Find(_ => true).ToListAsync();

    public async Task<List<EventModel>> FindByAggregateId(Guid aggregateId)
        => await _eventStoreCollection.Find(x => x.AggregateIndentifier == aggregateId).ToListAsync();

    public async Task SaveAsync(EventModel @event)
        => await _eventStoreCollection.InsertOneAsync(@event);
}
