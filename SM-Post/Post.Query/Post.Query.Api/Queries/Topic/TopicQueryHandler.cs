using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Api.Queries;

public class TopicQueryHandler : ITopicQueryHandler
{
    private readonly ITopicRepository _topicRepository;

    public TopicQueryHandler(ITopicRepository topicRepository) =>
        _topicRepository = topicRepository;

    public Task<List<Topic>> HandleAsync(GetAllTopicsQuery query) =>
        _topicRepository.GetAllAsync();
}