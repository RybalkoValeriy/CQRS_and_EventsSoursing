using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands.Topic;
using CQRS.Core.Commands;

namespace Post.Cmd.Api.Controllers;

[ApiController]
[Route("command/api/v1/[controller]")]
public class NewTopicController(ICommandResolver commandResolver) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> NewTopicAsync(NewTopicCommand command)
    {
        await commandResolver
            .ResolveFor<NewTopicCommand>()
            .SendAsync(command);

        return StatusCode(StatusCodes.Status202Accepted, command.Id);
    }
}