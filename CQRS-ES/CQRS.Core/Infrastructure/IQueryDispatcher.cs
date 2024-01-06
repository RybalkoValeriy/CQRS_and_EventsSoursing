using CQRS.Core.Queries;

namespace CQRS.Core.Infrastructure;

public interface IQueryDispatcher<TEntity>
{
    IQueryDispatcher<TEntity> RegisterHandler<TQuery>(Func<TQuery, Task<List<TEntity>>> handler) 
        where TQuery : BaseQuery;

    Task<List<TEntity>> SendAsync(BaseQuery query);
}