﻿using CQRS.Core.Events;

namespace Post.Common.Events.Topic;

public class TopicCreateEvent() : BaseEvent(nameof(TopicCreateEvent))
{
    public required string Name { get; set; } = string.Empty;
    public Guid AuthorId { get; set; }
}
