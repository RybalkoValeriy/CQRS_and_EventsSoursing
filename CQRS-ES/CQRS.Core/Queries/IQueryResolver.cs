using CQRS.Core.Handlers;

namespace CQRS.Core.Queries;

public interface IQueryResolver
{
    IQueryHandler<TQuery, TResult> ResolveFor<TQuery, TResult>() where TQuery : BaseQuery;
}