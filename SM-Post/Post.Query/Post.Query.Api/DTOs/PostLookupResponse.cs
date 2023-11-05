using Post.Common.DTOs;

namespace Post.Query.Api.DTOs;

public class PostLookupResponse : BaseResponse
{
    public List<Domain.Entities.Post> Posts { get; set; }
}