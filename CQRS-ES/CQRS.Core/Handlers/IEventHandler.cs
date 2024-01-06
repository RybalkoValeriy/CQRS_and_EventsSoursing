namespace CQRS.Core.Handlers;

public interface IEventHandler<in TQuery, TEntity>
    where TEntity : class
{
    Task<TEntity> SendAsync(TQuery @event, CancellationToken cancellationToken = default);
}