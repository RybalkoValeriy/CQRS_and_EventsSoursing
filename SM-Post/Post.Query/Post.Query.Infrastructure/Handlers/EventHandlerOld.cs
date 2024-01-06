using Post.Common.Events;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Infrastructure.Handlers;

/// <summary>
/// Base on event doing operation in sql - database
/// </summary>
public class EventHandlerOld(
    IPostRepository postRepository,
    ICommentRepository commentRepository)
    : IEventHandlerOld
{
    public async Task On(PostCreatedEvent @event)
    {
        var post = new Domain.Entities.Post
        {
            Id = @event.Id,
            Author = @event.Author,
            DatePosted = @event.DatePosted.ToUniversalTime(),
            Message = @event.Message
        };

        await postRepository.CreateAsync(post);
    }

    public async Task On(MessageUpdatedEvent @event)
    {
        var post = await postRepository.GetByIdAsync(@event.Id);

        post.Message = @event.Message;
        await postRepository.UpdateAsync(post);
    }

    public async Task On(PostLikedEvent @event)
    {
        var post = await postRepository.GetByIdAsync(@event.Id);

        post.Likes++;
        await postRepository.UpdateAsync(post);
    }

    public async Task On(CommentAddedEvent @event)
    {
        var comment = new Comment
        {
            PostId = @event.Id,
            Id = @event.CommentId,
            CommentDate = @event.CommentDate,
            CommentText = @event.Comment,
            UserName = @event.UserName,
            Edited = false
        };

        await commentRepository.CreateAsync(comment);
    }

    public async Task On(CommentUpdatedEvent @event)
    {
        var comment = await commentRepository.GetByIdAsync(@event.CommentId);

        comment.CommentText = @event.Comment;
        comment.Edited = true;
        comment.CommentDate = @event.EditDate;

        await commentRepository.UpdateAsync(comment);
    }

    public Task On(CommentRemovedEvent @event) =>
        commentRepository.DeleteAsync(@event.CommentId);

    public Task On(PostRemovedEvent @event) =>
        postRepository.DeleteAsync(@event.Id);
}