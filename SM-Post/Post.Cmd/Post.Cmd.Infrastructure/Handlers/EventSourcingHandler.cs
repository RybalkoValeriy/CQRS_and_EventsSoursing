using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producer;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Infrastructure.Handlers;


public class EventSourcingHandler : IEventSourcingHandler<PostAggregate>
{
    private readonly IEventStore _eventStore;
    private readonly IEventProducer _eventProducer;

    public EventSourcingHandler(
        IEventStore eventStore,
        IEventProducer eventProducer)
    {
        _eventProducer = eventProducer;
        _eventStore = eventStore;
    }

    public async Task<PostAggregate> GetByIdAsync(Guid aggregateId)
    {
        var postAggregate = new PostAggregate();
        var events = await _eventStore.GetAllEventsForAggregateAsync(aggregateId);

        if (events?.Any() is false)
            return postAggregate;

        postAggregate.ReplayEvent(events);
        postAggregate.Version = events.Max(x => x.Version);

        return postAggregate;
    }

    public async Task RepublishEventsAsync()
    {
        var aggregateIds = await _eventStore.GetAggregateIdsAsync();

        if (aggregateIds is null || aggregateIds.Any() is false) return;

        foreach (var aggregateId in aggregateIds)
        {
            var aggregate = await GetByIdAsync(aggregateId);

            if (aggregate is null || aggregate.Active is false)
                continue;

            var events = await _eventStore.GetAllEventsForAggregateAsync(aggregateId);

            foreach (var @event in events)
            {
                var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
                await _eventProducer.ProduceAsync(topic, @event);
            }
        }
    }

    public async Task SaveAsync(AggregateRoot aggregate)
    {
        await _eventStore.SaveEventsAsync
            (
                aggregate.Id,
                aggregate.GetUncommitedChanges(),
                aggregate.Version
            );

        aggregate.MarkChangesAsCommited();
    }
}
