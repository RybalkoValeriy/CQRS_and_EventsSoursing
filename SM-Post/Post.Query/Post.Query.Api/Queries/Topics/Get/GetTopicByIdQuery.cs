using CQRS.Core.Queries;

namespace Post.Query.Api.Queries.Topics;

public class GetTopicByIdQuery : BaseQuery
{
    public Guid TopicId { get; set; }
}