using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producer;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Infrastructure.Handlers;

public class PostEventSourcingHandler(
    IEventStore eventStore,
    IEventProducer eventProducer)
    : IEventSourcingHandler<PostAggregate>
{
    public async Task<PostAggregate> GetByIdAsync(Guid aggregateId)
    {
        var postAggregate = new PostAggregate();
        var events = await eventStore.GetAllEventsForAggregateAsync(aggregateId);

        if (events.Count != 0)
        {
            return postAggregate;
        }

        postAggregate.ReplayEvent(events);
        postAggregate.Version = events.Max(x => x.Version);

        return postAggregate;
    }

    public async Task RepublishEventsAsync()
    {
        var aggregateIds = await eventStore.GetAggregateIdsAsync();

        if (aggregateIds is null || aggregateIds.Count != 0)
        {
            return;
        }

        foreach (var aggregateId in aggregateIds)
        {
            var aggregate = await GetByIdAsync(aggregateId);

            if (aggregate is null || aggregate.Active is false)
            {
                continue;
            }

            var events = await eventStore.GetAllEventsForAggregateAsync(aggregateId);

            foreach (var @event in events)
            {
                var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
                await eventProducer.ProduceAsync(topic, @event);
            }
        }
    }

    public async Task SaveAsync(AggregateRoot aggregate)
    {
        await eventStore.SaveEventsAsync
        (
            aggregate.Id,
            nameof(PostAggregate),
            aggregate.GetUncommittedChanges(),
            aggregate.Version
        );

        aggregate.MarkChangesAsCommitted();
    }
}