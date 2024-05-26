using CQRS.Core.Handlers;
using Post.Common.Events.Topic;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Infrastructure.Handlers;

public class TopicCreatedEventHandler(ITopicRepository topicRepository)
    : IEventHandler<TopicCreateEvent, Topic>
{
    public async Task<Topic> SendAsync(TopicCreateEvent @event, CancellationToken cancellationToken = default)
    {
        var topic = new Topic
        {
            Id = @event.Id,
            Name = @event.Name,
            UserId = @event.AuthorId
        };

        await topicRepository.CreateAsync(topic, cancellationToken);

        return topic;
    }
}

public class 