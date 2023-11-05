using Post.Common.Events;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Infrastructure.Handlers;

public class EventHandler : IEventHandler
{
    private readonly IPostRepository _postRepository;
    private readonly ICommentRepository _commentRepository;

    public EventHandler(
        IPostRepository postRepository,
        ICommentRepository commentRepository)
    {
        _postRepository = postRepository;
        _commentRepository = commentRepository;
    }

    public async Task On(PostCreatedEvent @event)
    {
        var post = new Domain.Entities.Post
        {
            Id = @event.Id,
            Author = @event.Auther,
            DatePosted = @event.DatePosted.ToUniversalTime(),
            Message = @event.Message
        };

        await _postRepository.CreateAsync(post);
    }

    public async Task On(MessageUpdatedEvent @event)
    {
        var post = await _postRepository.GetByIdAsync(@event.Id);

        if (post == null) return;

        post.Message = @event.Message;
        await _postRepository.UpdateAsync(post);
    }

    public async Task On(PostLikedEvent @event)
    {
        var post = await _postRepository.GetByIdAsync(@event.Id);

        if (post == null) return;

        post.Likes++;
        await _postRepository.UpdateAsync(post);
    }

    public async Task On(CommentAddedEvent @event)
    {
        var comment = new Comment
        {
            PostId = @event.Id,
            CommentId = @event.CommentId,
            CommentDate = @event.CommentDate,
            CommentText = @event.Comment,
            UserName = @event.UserName,
            Edited = false
        };

        await _commentRepository.CreateAsync(comment);
    }

    public async Task On(CommentUpdatedEvent @event)
    {
        var comment = await _commentRepository.GetByIdAsync(@event.CommentId);

        if (comment == null) return;

        comment.CommentText = @event.Comment;
        comment.Edited = true;
        comment.CommentDate = @event.EditDate;

        await _commentRepository.UpdateAsync(comment);
    }

    public Task On(CommentRemovedEvent @event)
        => _commentRepository.DeleteAsync(@event.CommentId);

    public Task On(PostRemovedEvent @event)
        => _postRepository.DeleteAsync(@event.Id);
}