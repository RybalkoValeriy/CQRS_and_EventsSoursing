using CQRS.Core.Handlers;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Api.Queries;

public class GetAllTopicsQueryHandler(ITopicRepository topicRepository)
    : IQueryHandler<GetAllTopicsQuery, List<Topic>>
{
    public Task<List<Topic>> SendAsync(GetAllTopicsQuery query, CancellationToken cancellationToken) =>
        topicRepository.GetAllAsync(cancellationToken);
}