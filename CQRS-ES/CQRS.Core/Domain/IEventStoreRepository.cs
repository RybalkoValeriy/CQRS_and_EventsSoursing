using CQRS.Core.Events;

namespace CQRS.Core.Domain;
public interface IEventStoreRepository // should consist only immutable data 
{
    Task SaveAsync(EventModel @event);
    Task<List<EventModel>> FingByAggregateId(Guid aggregateId);
}
