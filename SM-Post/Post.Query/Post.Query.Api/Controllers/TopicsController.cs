using CQRS.Core.Queries;
using Microsoft.AspNetCore.Mvc;
using Post.Query.Api.Queries.Topics;
using Post.Query.Domain.Entities;

namespace Post.Query.Api.Controllers;

[ApiController]
[Route("query/api/v1/[controller]")]
public class TopicsController(IQueryResolver queryResolver) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var topics = await queryResolver
            .ResolveFor<GetAllTopicsQuery, List<Topic>>()
            .SendAsync(new GetAllTopicsQuery());

        return Ok(topics);
    }

    [HttpGet("{topicId:guid}")]
    public async Task<ActionResult<Topic>> Get([FromRoute] Guid topicId)
    {
        var topic = await queryResolver
            .ResolveFor<GetTopicByIdQuery, Topic>()
            .SendAsync(new GetTopicByIdQuery { TopicId = topicId });

        return Ok(topic);
    }
}