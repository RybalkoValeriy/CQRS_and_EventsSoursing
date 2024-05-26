using CQRS.Core.Handlers;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;


namespace Post.Query.Api.Queries.Topics;

public class GetTopicByIdQueryHandler(ITopicRepository topicRepository) : IQueryHandler<GetTopicByIdQuery, Topic>
{
    public Task<Topic> SendAsync(GetTopicByIdQuery query, CancellationToken cancellationToken = default) =>
        topicRepository.GetTopicAsync(query.TopicId, cancellationToken);
}
