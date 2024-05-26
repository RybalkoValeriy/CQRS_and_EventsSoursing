using CQRS.Core.Handlers;
using CQRS.Core.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Post.Query.Infrastructure.Resolver;

public class QueryResolver(IServiceProvider serviceProvider) : IQueryResolver
{
    public IQueryHandler<TQuery, TResult> ResolveFor<TQuery, TResult>() where TQuery : BaseQuery =>
        serviceProvider.GetService<IQueryHandler<TQuery, TResult>>() ?? throw new Exception("can't resolve event");
}