using CQRS.Core.Handlers;
using Post.Cmd.Api.Commands.Topic;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Api.Commands;

/// <summary>
/// 
/// </summary>
public class CommandHandler(
    IEventSourcingHandler<PostAggregate> postEventSourcingHandler,
    IEventSourcingHandler<TopicAggregate> topicEventSourcingHandler)
    : ICommandHandler
{
    public async Task HandlerAsync(AddCommentCommand command)
    {
        var postAggregate = await postEventSourcingHandler.GetByIdAsync(command.Id);
        postAggregate.AddComment(command.Comment, command.UserName);

        await postEventSourcingHandler.SaveAsync(postAggregate);
    }

    public async Task HandlerAsync(EditCommentCommand command)
    {
        var postAggregate = await postEventSourcingHandler.GetByIdAsync(command.Id);
        postAggregate.EditComment(command.CommentId, command.Comment, command.UserName);

        await postEventSourcingHandler.SaveAsync(postAggregate);
    }

    public async Task HandlerAsync(RemoveCommentCommand command)
    {
        var postAggregate = await postEventSourcingHandler.GetByIdAsync(command.Id);
        postAggregate.RemoveComment(command.CommentId);

        await postEventSourcingHandler.SaveAsync(postAggregate);
    }

    public async Task HandlerAsync(NewPostCommand command)
    {
        var postAggregate = new PostAggregate(command.Id, command.Author, command.Message);

        await postEventSourcingHandler.SaveAsync(postAggregate);
    }

    public async Task HandlerAsync(NewTopicCommand command)
    {
        var topicAggregate = new TopicAggregate(command.Id, command.Name, command.AuthorId);

        await topicEventSourcingHandler.SaveAsync(topicAggregate);
    }

    public async Task HandlerAsync(LikePostCommand command)
    {
        var postAggregate = await postEventSourcingHandler.GetByIdAsync(command.Id);
        postAggregate.LikePost();

        await postEventSourcingHandler.SaveAsync(postAggregate);
    }

    public async Task HandlerAsync(DeletePostCommand command)
    {
        var postAggregate = await postEventSourcingHandler.GetByIdAsync(command.Id);
        postAggregate.RemovePost();

        await postEventSourcingHandler.SaveAsync(postAggregate);
    }

    public async Task HandlerAsync(EditMessageCommand command)
    {
        var postAggregate = await postEventSourcingHandler.GetByIdAsync(command.Id);
        postAggregate.EditMessage(command.Message);

        await postEventSourcingHandler.SaveAsync(postAggregate);
    }

    public Task HandlerAsync(RestoreReadDbCommand command) =>
        postEventSourcingHandler.RepublishEventsAsync();
}