using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands;

public record RemoveCommentCommand : BaseCommand
{
    public Guid CommentId { get; set; }
    public string UserName { get; set; } = string.Empty;
}
