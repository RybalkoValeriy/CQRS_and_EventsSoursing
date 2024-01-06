using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producer;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Infrastructure.Handlers;

/// <summary>
/// Handler a aggregation between topic
/// </summary>
public class TopicEventSourcingHandler : IEventSourcingHandler<TopicAggregate>
{
    private readonly IEventStore _eventStore;
    private readonly IEventProducer _eventProducer;

    public TopicEventSourcingHandler(
        IEventStore eventStore,
        IEventProducer eventProducer)
    {
        _eventProducer = eventProducer;
        _eventStore = eventStore;
    }

    public async Task SaveAsync(AggregateRoot aggregate)
    {
        await _eventStore
            .SaveEventsAsync(
                aggregate.Id,
                nameof(TopicAggregate),
                aggregate.GetUncommittedChanges(),
                aggregate.Version);

        aggregate.MarkChangesAsCommitted();
    }

    public async Task<TopicAggregate> GetByIdAsync(Guid aggregateId)
    {
        var topicAggregate = new TopicAggregate();
        var events = await _eventStore.GetAllEventsForAggregateAsync(aggregateId);

        if (events?.Any() is false)
        {
            return topicAggregate;
        }

        topicAggregate.ReplayEvent(events);
        topicAggregate.Version = events!.Max(x => x.Version);

        return topicAggregate;
    }

    /// <summary>
    /// use case: Restore database based on event sourcing DB
    /// </summary>
    public async Task RepublishEventsAsync()
    {
        var aggregateIds = await _eventStore.GetAggregateIdsAsync();

        if (aggregateIds is null || aggregateIds.Any() is false)
        {
            return;
        }

        foreach (var aggregateId in aggregateIds)
        {
            var aggregate = await GetByIdAsync(aggregateId);

            if (aggregate is null)
            {
                continue;
            }

            var events = await _eventStore.GetAllEventsForAggregateAsync(aggregateId);

            foreach (var @event in events)
            {
                var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
                await _eventProducer.ProduceAsync(topic, @event);
            }
        }
    }
}