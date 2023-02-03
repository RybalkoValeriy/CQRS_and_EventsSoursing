using CQRS.Core.Events;

namespace CQRS.Core.Infrastructure;
public interface IEventStore // provide us access to EventStore business logic
{
    Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion);
    Task<List<BaseEvent>> GetAllEventsForAggregateAsync(Guid aggregateId);
}
