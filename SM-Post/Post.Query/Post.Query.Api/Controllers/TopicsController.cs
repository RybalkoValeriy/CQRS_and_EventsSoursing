using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Query.Api.DTOs;
using Post.Query.Api.Queries;
using Post.Query.Domain.Entities;

namespace Post.Query.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TopicsController : ControllerBase
{
    private readonly ILogger<PostLookupController> _logger;
    private readonly IQueryDispatcher<Topic> _topicQueryDispatcher;

    public TopicsController(
        IQueryDispatcher<Topic> topicQueryDispatcher,
        ILogger<PostLookupController> logger)
    {
        _topicQueryDispatcher = topicQueryDispatcher;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var topics = await _topicQueryDispatcher.SendAsync(new GetAllTopicsQuery());

        return Ok(
            new TopicsResponse
            {
                Message = "Successfully returned",
                Type = topics
            });
    }
}