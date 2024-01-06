using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Api.Queries;

public class TopicQueryHandler(ITopicRepository topicRepository) : ITopicQueryHandler
{
    public Task<List<Topic>> HandleAsync(GetAllTopicsQuery query) =>
        topicRepository.GetAllAsync();

    public Task<Topic> HandleAsync(GetTopicByIdQuery query) =>
        topicRepository.GetTopic(query.TopicId);
}