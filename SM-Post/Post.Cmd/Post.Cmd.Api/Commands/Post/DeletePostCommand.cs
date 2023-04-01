using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands;

public record DeletePostCommand : BaseCommand
{
    public string UserName { get; set; } = string.Empty;
}
