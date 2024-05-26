using Post.Query.Domain.Entities;

namespace Post.Query.Domain.Repositories;

public interface ITopicRepository
{
    Task<List<Topic>> GetAllAsync(CancellationToken cancellationToken = default);

    Task CreateAsync(Topic topic, CancellationToken cancellationToken = default);

    Task<Topic> GetTopicAsync(Guid topicId, CancellationToken cancellationToken = default);
}