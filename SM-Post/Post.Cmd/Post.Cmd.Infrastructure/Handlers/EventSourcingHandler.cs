using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Infrastructure.Handlers;


public class EventSourcingHandler : IEventSourcingHandler<PostAggregate>
{
    private readonly IEventStore _eventStore;

    public EventSourcingHandler(IEventStore eventStore)
        => _eventStore = eventStore;

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
