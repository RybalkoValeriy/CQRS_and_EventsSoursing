using CQRS.Core.Events;

namespace Post.Common.Events;

public class CommentUpdatedEvent() : BaseEvent(nameof(CommentUpdatedEvent))
{
    public Guid CommentId { get; set; }
    public string Comment { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public DateTime EditDate { get; set; }
}
