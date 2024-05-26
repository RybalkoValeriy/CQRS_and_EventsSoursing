using CQRS.Core.Handlers;
using Post.Query.Domain.Repositories;
using Post.Query.Domain.Entities;

namespace Post.Query.Api.Queries.Topics;

public class GetAllTopicsQueryHandler(ITopicRepository topicRepository) : IQueryHandler<GetAllTopicsQuery, List<Topic>>
{
    public Task<List<Topic>> SendAsync(GetAllTopicsQuery query, CancellationToken cancellationToken) =>
        topicRepository.GetAllAsync(cancellationToken);
}
