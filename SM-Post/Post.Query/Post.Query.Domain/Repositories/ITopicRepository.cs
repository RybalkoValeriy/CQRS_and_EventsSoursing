using Post.Query.Domain.Entities;

namespace Post.Query.Domain.Repositories;

public interface ITopicRepository
{
    Task<List<Topic>> GetAllAsync();
}