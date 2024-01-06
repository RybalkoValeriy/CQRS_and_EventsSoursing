using CQRS.Core.Events;

namespace CQRS.Core.Infrastructure;

// provide us access to EventStore business logic
public interface IEventStore
{
    Task SaveEventsAsync(Guid aggregateId, string aggregateType, IEnumerable<BaseEvent> events, int expectedVersion);

    Task<List<BaseEvent>> GetAllEventsForAggregateAsync(Guid aggregateId);

    Task<List<Guid>> GetAggregateIdsAsync();
}