//using CQRS.Core.Commands;
//using Microsoft.AspNetCore.Mvc;
//using Post.Cmd.Api.Commands;
//using Post.Common.DTOs;

//namespace Post.Cmd.Api.Controllers;

//[ApiController]
//[Route("command/api/v1/[controller]")]
//public class AddCommentController(ILogger<AddCommentController> logger, ICommandDispatcher commandDispatcher) : ControllerBase
//{
//    [HttpPut("{id:guid}")]
//    public async Task<ActionResult> AddCommentAsync(Guid id, AddCommentCommand command)
//    {
//        try
//        {
//            await commandDispatcher.SendAsync(command);

//            return Ok(new BaseResponse
//            {
//                Message = "Add comment request completed successfully!"
//            });
//        }
//        catch (InvalidOperationException ex)
//        {
//            logger.Log(LogLevel.Warning, ex, "Client made a bad request!");
//            return BadRequest(new BaseResponse
//            {
//                Message = ex.Message
//            });
//        }
//        // catch (AggregateNotFoundException ex)
//        // {
//        //     _logger.Log(LogLevel.Warning, ex, "Could not retrieve aggregate, client passed an incorrect post ID targetting the aggregate!");
//        //     return BadRequest(new BaseResponse
//        //     {
//        //         Message = ex.Message
//        //     });
//        // }
//        catch (Exception ex)
//        {
//            const string SAFE_ERROR_MESSAGE = "Error while processing request to add a comment to a post!";
//            logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);

//            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
//            {
//                Message = SAFE_ERROR_MESSAGE
//            });
//        }
//    }
//}
