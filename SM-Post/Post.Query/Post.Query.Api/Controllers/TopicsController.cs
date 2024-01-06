using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Query.Api.Queries;
using Post.Query.Domain.Entities;

namespace Post.Query.Api.Controllers;

[ApiController]
[Route("query/api/v1/[controller]")]
public class TopicsController(
    IQueryResolver queryResolver,
    ILogger<PostLookupController> logger)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // todo: continue implementation 

        var topics = await queryResolver
            .ResolveFor<GetAllTopicsQuery, List<Topic>>()
            .SendAsync(new GetAllTopicsQuery());

        return Ok(topics);
    }

    // [HttpGet("{topicId:guid}")]
    // public async Task<ActionResult<Topic>> Get([FromRoute] Guid topicId)
    // {
    //     var topic = await queryResolver.SendAsync(
    //         new GetTopicByIdQuery
    //         {
    //             TopicId = topicId
    //         });
    //
    //     if (topic is not null)
    //     {
    //         return Ok(topic);
    //     }
    //
    //     logger.LogWarning("topic id:{topicId} is not found", topicId);
    //     return new NotFoundResult();
    // }
}