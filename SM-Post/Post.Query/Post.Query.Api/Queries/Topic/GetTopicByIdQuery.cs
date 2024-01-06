using CQRS.Core.Queries;

namespace Post.Query.Api.Queries;

public class GetTopicByIdQuery : BaseQuery
{
    public Guid TopicId { get; set; }
}