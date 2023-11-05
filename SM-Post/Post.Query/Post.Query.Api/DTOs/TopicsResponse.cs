using Post.Common.DTOs;
using Post.Query.Domain.Entities;

namespace Post.Query.Api.DTOs;

public class TopicsResponse : BaseResponse
{
    public List<Topic> Type { get; set; }
}