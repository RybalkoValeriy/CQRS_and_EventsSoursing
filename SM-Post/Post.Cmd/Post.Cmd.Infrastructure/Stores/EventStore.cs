using CQRS.Core.Domain;
using CQRS.Core.Events;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producer;

namespace Post.Cmd.Infrastructure.Stores;

/// <summary>
/// Save to EventStore(RepoMongoDb) and publish event to Kafka
/// </summary>
public class EventStore(
    IEventStoreRepository eventStoreRepository,
    IEventProducer eventProducer)
    : IEventStore
{
    public async Task<List<Guid>> GetAggregateIdsAsync()
    {
        var eventStream = await eventStoreRepository.FindAllAsync();

        if (eventStream is not { Count: 0 })
        {
            throw new Exception("Null");
        }

        return eventStream
            .Select(x => x.AggregateIdentifier)
            .Distinct()
            .ToList();
    }

    public async Task<List<BaseEvent>> GetAllEventsForAggregateAsync(Guid aggregateId)
    {
        var eventStream = await eventStoreRepository.FindByAggregateId(aggregateId);

        if (eventStream is not { Count: 0 })
        {
            throw new Exception("Incorrect post Id provider");
        }

        return eventStream
            .OrderBy(x => x.Version)
            .Select(x => x.EventData)
            .ToList();
    }

    public async Task SaveEventsAsync(
        Guid aggregateId,
        string aggregateType,
        IEnumerable<BaseEvent> events,
        int expectedVersion)
    {
        var eventStream = await eventStoreRepository.FindByAggregateId(aggregateId);

        // check if last version as an expected version then we can create a new version
        if (expectedVersion != -1 && eventStream[^1].Version != expectedVersion)
        {
            throw new Exception("Concurrency exception"); // optimistic
        }

        var version = expectedVersion;

        foreach (var @event in events)
        {
            version++;
            @event.Version = version;
            var eventType = @event.GetType().Name;

            var eventModel = new EventModel
            {
                TimeStamp = DateTime.Now,
                AggregateIdentifier = aggregateId,
                AggregateType = aggregateType,
                Version = version,
                EventType = eventType,
                EventData = @event
            };

            await eventStoreRepository.SaveAsync(eventModel);

            var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
            await eventProducer.ProduceAsync(topic, @event);
        }
    }
}