using Post.Cmd.Api.Commands.Topic;

namespace Post.Cmd.Api.Commands;
public interface ICommandHandler
{
    Task HandlerAsync(AddCommentCommand command);
    Task HandlerAsync(EditCommentCommand command);
    Task HandlerAsync(RemoveCommentCommand command);

    Task HandlerAsync(NewPostCommand command);
    Task HandlerAsync(LikePostCommand command);
    Task HandlerAsync(DeletePostCommand command);

    Task HandlerAsync(EditMessageCommand command);
    Task HandlerAsync(RestoreReadDbCommand command);

    Task HandlerAsync(NewTopicCommand command);
}
