using Post.Query.Api.Queries.Topics;
using Post.Query.Domain.Entities;

namespace Post.Query.Api.Queries;

public interface ITopicQueryHandler
{
    Task<List<Topic>> HandleAsync(GetAllTopicsQuery query);
    Task<Topic> HandleAsync(GetTopicByIdQuery query);
}