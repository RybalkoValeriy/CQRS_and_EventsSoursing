using CQRS.Core.Handlers;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Api.Commands;

public class CommandHandler : ICommandHandler
{
    private readonly IEventSourcingHandler<PostAggregate> _eventSourcingHandler;

    public CommandHandler(IEventSourcingHandler<PostAggregate> eventSourcingHandler)
    => _eventSourcingHandler = eventSourcingHandler;

    public async Task HandlerAsync(AddCommentCommand command)
    {
        var postAggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
        postAggregate.AddComment(command.Comment, command.UserName);

        await _eventSourcingHandler.SaveAsync(postAggregate);
    }

    public async Task HandlerAsync(EditCommentCommand command)
    {
        var postAggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
        postAggregate.EditComment(command.CommentId, command.Comment, command.UserName);

        await _eventSourcingHandler.SaveAsync(postAggregate);
    }

    public async Task HandlerAsync(RemoveCommentCommand command)
    {
        var postAggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
        postAggregate.RemoveComment(command.CommentId);

        await _eventSourcingHandler.SaveAsync(postAggregate);
    }

    public async Task HandlerAsync(NewPostCommand command)
    {
        var postAggregate = new PostAggregate(command.Id, command.Author, command.Message);

        await _eventSourcingHandler.SaveAsync(postAggregate);
    }

    public async Task HandlerAsync(LikePostCommand command)
    {
        var postAggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
        postAggregate.LikePost();

        await _eventSourcingHandler.SaveAsync(postAggregate);
    }

    public async Task HandlerAsync(DeletePostCommand command)
    {
        var postAggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
        postAggregate.RemovePost(command.UserName);

        await _eventSourcingHandler.SaveAsync(postAggregate);
    }

    public async Task HandlerAsync(EditMessageCommand command)
    {
        var postAggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
        postAggregate.EditMessage(command.Message);

        await _eventSourcingHandler.SaveAsync(postAggregate);
    }

    public async Task HandlerAsync(RestoreReadDbCommand command) => await _eventSourcingHandler.RepublishEventsAsync();
}
