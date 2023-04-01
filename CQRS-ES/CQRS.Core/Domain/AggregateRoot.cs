using CQRS.Core.Events;

namespace CQRS.Core.Domain;

public abstract class AggregateRoot
{
    protected Guid _id;

    // state changes of the aggregate
    private readonly List<BaseEvent> _changes = new();

    public Guid Id => _id;

    public int Version { get; set; } = -1;

    public IEnumerable<BaseEvent> GetUncommitedChanges()
        => _changes;

    public void MarkChangesAsCommited()
        => _changes.Clear();

    public void ApplyChange(BaseEvent @event, bool isNew)
    {
        var method = this
            .GetType()
            .GetMethod("Apply", new Type[] { @event.GetType() });

        if (method is null)
            throw new ArgumentNullException(nameof(method), $"Apply method is not found in aggregate for");

        method.Invoke(this, new object[] { @event });

        if (isNew)
            _changes.Add(@event);
    }

    protected void RaiseEvent(BaseEvent @event)
        => ApplyChange(@event, true);

    // use for apply all changes for postAggregate afteg fetching
    public void ReplayEvent(IEnumerable<BaseEvent> events)
    {
        foreach (var @event in events)
        {
            ApplyChange(@event, false);
        }
    }
}
