using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands;

public record AddCommentCommand : BaseCommand
{
    public string Comment { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
}
