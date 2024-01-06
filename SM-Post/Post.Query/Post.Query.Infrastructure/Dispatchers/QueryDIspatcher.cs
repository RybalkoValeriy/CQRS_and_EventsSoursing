using CQRS.Core.Infrastructure;
using CQRS.Core.Queries;

namespace Post.Query.Infrastructure.Dispatchers;
public class QueryDispatcher<TEntity> : IQueryDispatcher<TEntity>
{
    private readonly Dictionary<Type, Func<BaseQuery, Task<List<TEntity>>>> _handlers = new();

    public IQueryDispatcher<TEntity> RegisterHandler<TQuery>(Func<TQuery, Task<List<TEntity>>> handler) 
        where TQuery : BaseQuery
    {
        if (_handlers.ContainsKey(typeof(TQuery)))
        {
            throw new IndexOutOfRangeException("You cannot register the same query handler twice!");
        }

        _handlers.Add(typeof(TQuery), x => handler((TQuery)x));

        return this;
    }

    public async Task<List<TEntity>> SendAsync(BaseQuery query)
    {
        if (_handlers.TryGetValue(query.GetType(), out var handler))
        {
            return await handler(query);
        }

        throw new ArgumentNullException(nameof(handler), "No query handler was registered!");
    }
}


