using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands.Topic;

namespace Post.Cmd.Api.Controllers;

[ApiController]
[Route("command/api/v1/[controller]")]
public class NewTopicController(
    ILogger<NewPostController> logger,
    ICommandDispatcher commandDispatcher) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> NewTopicAsync(NewTopicCommand command)
    {
        await commandDispatcher.SendAsync(command);

        return StatusCode(StatusCodes.Status202Accepted, command.Id);
    }
}