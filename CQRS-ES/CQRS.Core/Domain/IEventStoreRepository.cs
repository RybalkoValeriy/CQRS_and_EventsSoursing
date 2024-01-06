using CQRS.Core.Events;

namespace CQRS.Core.Domain;

// should consist only immutable data
public interface IEventStoreRepository
{
    Task SaveAsync(EventModel @event);

    Task<List<EventModel>> FindByAggregateId(Guid aggregateId);

    Task<List<EventModel>> FindAllAsync();
}
