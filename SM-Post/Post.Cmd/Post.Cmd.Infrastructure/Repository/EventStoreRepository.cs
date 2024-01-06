using CQRS.Core.Domain;
using CQRS.Core.Events;
using MongoDB.Driver;

namespace Post.Cmd.Infrastructure.Repository;

/// <summary>
/// Operation with mongoDb + connection to mongoDb
/// </summary>
public class EventStoreRepository : IEventStoreRepository
{
    private readonly IMongoCollection<EventModel> _eventStoreCollection;

    public EventStoreRepository()
    {
        // should be from IOptions<MongoDbOptions>
        var connectionString = "mongodb://localhost:27017";
        var dataBase = "socialMedia";
        var collection = "eventStore";

        var mongoClient = new MongoClient(connectionString);
        var mongoDatabase = mongoClient.GetDatabase(dataBase);

        _eventStoreCollection = mongoDatabase.GetCollection<EventModel>(collection);
    }

    public Task<List<EventModel>> FindAllAsync() =>
        _eventStoreCollection.Find(_ => true).ToListAsync();

    public Task<List<EventModel>> FindByAggregateId(Guid aggregateId) =>
        _eventStoreCollection.Find(x => x.AggregateIdentifier == aggregateId).ToListAsync();

    public Task SaveAsync(EventModel @event) =>
        _eventStoreCollection.InsertOneAsync(@event);
}