using CQRS.Core.Domain;
using Post.Common.Events.Topic;

namespace Post.Cmd.Domain.Aggregates;

public class TopicAggregate : AggregateRoot
{
    private Guid _authorId;

    public TopicAggregate()
    {
    }

    public TopicAggregate(
        Guid id,
        string name,
        Guid authorId)
    {
        RaiseEvent(
            new TopicCreateEvent
            {
                Id = id,
                Name = name,
                AuthorId = authorId
            });
    }

    public void Apply(TopicCreateEvent @event)
    {
        _id = @event.Id;
        _authorId = @event.AuthorId;
    }
}