using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands.Topic;

public class NewTopicCommand : BaseCommand
{
    public string Name { get; set; }
    public Guid AuthorId { get; set; }
}