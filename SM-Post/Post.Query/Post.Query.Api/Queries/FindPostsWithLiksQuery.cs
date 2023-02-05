using CQRS.Core.Queries;

namespace Post.Query.Api.Queries;

public class FindPostsWithLiksQuery : BaseQuery
{
    public int NumberOfLikes { get; set; }
}
