using CQRS.Core.Handlers;
using CQRS.Core.Queries;

namespace CQRS.Core.Infrastructure;

public interface IQueryResolver
{
    IQueryHandler<TQuery, TResult> ResolveFor<TQuery, TResult>() where TQuery : BaseQuery;
}