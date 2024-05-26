//using CQRS.Core.Commands;
//using Microsoft.AspNetCore.Mvc;
//using Post.Cmd.Api.Commands;
//using Post.Cmd.Api.DTOs;
//using Post.Common.DTOs;

//namespace Post.Cmd.Api.Controllers;
//[ApiController]
//[Route("command/api/v1/[controller]")]
//public class NewPostController(
//    ILogger<NewPostController> logger,
//    ICommandDispatcher commandDispatcher) : ControllerBase
//{
//    [HttpPost]
//    public async Task<ActionResult> NewPostAsync(NewPostCommand command)
//    {
//        var id = Guid.NewGuid();
//        try
//        {
//            await commandDispatcher.SendAsync(command);

//            return StatusCode(StatusCodes.Status201Created, new NewPostResponse
//            {
//                Id = id,
//                Message = "New post creation request completed successfully!"
//            });
//        }
//        catch (InvalidOperationException ex)
//        {
//            logger.Log(LogLevel.Warning, ex, "Client made a bad request!");
//            return BadRequest(
//                new BaseResponse
//                {
//                    Message = ex.Message
//                });
//        }
//        catch (Exception ex)
//        {
//            const string SAFE_ERROR_MESSAGE = "Error while processing request to create a new post!";
//            logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);

//            return StatusCode(StatusCodes.Status500InternalServerError, new NewPostResponse
//            {
//                Id = id,
//                Message = SAFE_ERROR_MESSAGE
//            });
//        }

//    }
//}
