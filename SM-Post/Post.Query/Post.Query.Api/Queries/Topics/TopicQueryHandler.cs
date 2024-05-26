using Post.Query.Api.Queries.Topics;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Api.Queries;

public class TopicQueryHandler(ITopicRepository topicRepository) : ITopicQueryHandler
{
    public Task<List<Topic>> HandleAsync(GetAllTopicsQuery _) =>
        topicRepository.GetAllAsync();

    public Task<Topic> HandleAsync(GetTopicByIdQuery query) =>
        topicRepository.GetTopicAsync(query.TopicId);
}