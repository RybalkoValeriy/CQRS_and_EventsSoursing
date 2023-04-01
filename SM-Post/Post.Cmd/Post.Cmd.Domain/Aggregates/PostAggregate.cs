using CQRS.Core.Domain;
using Post.Common.Events;

namespace Post.Cmd.Domain.Aggregates;

public class PostAggregate : AggregateRoot
{
    private bool _active;

    private string _author;

    private readonly Dictionary<Guid, Tuple<string, string>> _comments = new();

    public bool Active
    {
        get => _active;
        set => _active = value;
    }

    public PostAggregate()
    {
    }

    public PostAggregate(
        Guid guid,
        string author,
        string message)
        => RaiseEvent(
            new PostCreatedEvent
            {
                Id = guid,
                Auther = author,
                Message = message,
                DatePosted = DateTime.Now
            });

    public void Apply(PostCreatedEvent @event)
    {
        _id = @event.Id;
        _active = true;
        _author = @event.Auther;
    }

    public void EditMessage(string message)
    {
        if (_active is false)
        {
            throw new InvalidOperationException("Can't add inactive post");
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentNullException("The message should't be null");
        }

        RaiseEvent(
            new MessageUpdatedEvent
            {
                Id = _id,
                Message = message
            });
    }

    public void Apply(MessageUpdatedEvent @event)
        => _id = @event.Id;

    public void LikePost()
    {
        if (_active is false)
        {
            throw new Exception("the post should be active");
        }

        RaiseEvent(new PostLikedEvent {Id = _id});
    }

    public void Apply(PostLikedEvent @event)
        => _id = @event.Id;

    public void AddComment(string comment, string userName)
    {
        if (_active is false)
        {
            throw new InvalidOperationException("the post should be active");
        }

        if (string.IsNullOrWhiteSpace(comment))
        {
            throw new ArgumentNullException("The comment should't be null");
        }


        RaiseEvent(
            new CommentAddedEvent
            {
                Id = _id,
                CommentId = Guid.NewGuid(),
                Comment = comment,
                UserName = userName,
                CommentDate = DateTime.Now,
            });
    }

    public void Apply(CommentAddedEvent @event)
    {
        _id = @event.Id;

        _comments.Add(@event.CommentId, new Tuple<string, string>(@event.Comment, @event.UserName));
    }

    public void EditComment(Guid commetId, string comment, string userName)
    {
        if (_active is false)
            throw new InvalidOperationException("the post should be active");

        if (_comments[commetId].Item2.Equals(userName, StringComparison.CurrentCultureIgnoreCase) is false)
            throw new InvalidOperationException("The comment dose't exists");

        RaiseEvent(new CommentUpdatedEvent
        {
            Id = _id,
            CommentId = commetId,
            Comment = comment,
            UserName = userName,
            EditDate = DateTime.Now,
        });
    }

    public void Apply(CommentUpdatedEvent @event)
    {
        _id = @event.Id;
        _comments[@event.CommentId] = new Tuple<string, string>(@event.Comment, @event.UserName);
    }

    public void RemoveComment(Guid commentId)
    {
        if (_active is false)
            throw new InvalidOperationException("the post should be active");


        RaiseEvent(
            new CommentRemovedEvent
            {
                Id = _id,
                CommentId = commentId
            });

        _comments.Remove(commentId);
    }

    public void Apply(CommentRemovedEvent @event)
    {
        _id = @event.Id;
        _comments.Remove(@event.CommentId);
    }

    public void RemovePost(string userName)
    {
        if (_active is false)
            throw new InvalidOperationException("the post should be active");

        RaiseEvent(new PostRemovedEvent {Id = _id});
    }

    public void Apply(PostRemovedEvent @event)
    {
        _id = @event.Id;
        _active = false;
    }
}
