using CQRS.Core.Domain;
using CQRS.Core.Events;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producer;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Infrastructure.Stores;
public class EventStore : IEventStore
{
    private readonly IEventStoreRepository _eventStoreRepository;
    private readonly IEventProducer _eventProducer;

    public EventStore(
        IEventStoreRepository eventStoreRepostiry,
        IEventProducer eventProducer)
    {
        _eventStoreRepository = eventStoreRepostiry;
        _eventProducer = eventProducer;
    }

    public async Task<List<Guid>> GetAggregateIdsAsync()
    {
        var eventStream = await _eventStoreRepository.FindAllAsync();

        if (eventStream == null || !eventStream.Any())
            throw new Exception("Null");

        return eventStream.Select(x => x.AggregateIndentifier).Distinct().ToList();
    }

    public async Task<List<BaseEvent>> GetAllEventsForAggregateAsync(Guid aggregateId)
    {
        var eventStream = await _eventStoreRepository.FingByAggregateId(aggregateId);

        if (eventStream.Any() is false) throw new Exception("Incorrect post Id provider");

        return eventStream.OrderBy(x => x.Version).Select(x => x.EventData).ToList();
    }

    public async Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
    {
        var eventStream = await _eventStoreRepository.FingByAggregateId(aggregateId);

        // check if last version as a expected version then we can create a new version
        if (expectedVersion != -1 && eventStream[^1].Version != expectedVersion)
            throw new Exception("Concurrency exception"); // optimistic

        var version = expectedVersion;

        foreach (var @event in events)
        {
            version++;
            @event.Version = version;
            var eventType = @event.GetType().Name;

            var eventModel = new EventModel
            {
                TimeStamp = DateTime.Now,
                AggregateIndentifier = aggregateId,
                AggregateType = nameof(PostAggregate),
                Version = version,
                EventType = eventType,
                EventData = @event
            };

            await _eventStoreRepository.SaveAsync(eventModel);

            var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
            await _eventProducer.ProduceAsync(topic, @event);
        }
    }
}
