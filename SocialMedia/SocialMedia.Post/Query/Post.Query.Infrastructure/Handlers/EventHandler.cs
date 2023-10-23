using Post.Common.Events;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Infrastructure.Handlers;

public class EventHandler : IEventHandler
{
    private readonly IPostRepository _postRepository;
    private readonly ICommentRepository _commentRepository;

    public EventHandler(IPostRepository postRepository, ICommentRepository commentRepository)
    {
        _postRepository = postRepository;
        _commentRepository = commentRepository;
    }
    
    public async Task On(PostCreatedEvent @event)
    {
        await _postRepository.CreateAsync(new PostEntity
        {
            PostId = @event.Id,
            Author = @event.Author,
            Text = @event.Text,
            CreateAt = @event.CreateAt
        });
    }

    public async Task On(PostEditedEvent @event)
    {
        var post = await _postRepository.GetByIdAsync(@event.Id);
        
        if(post is null) return;

        post.Text = @event.Text;
        
        await _postRepository.UpdateAsync(post);
    }

    public async Task On(PostLikedEvent @event)
    {
        var post = await _postRepository.GetByIdAsync(@event.Id);
        if (post is null) return;
        post.Likes++;
        await _postRepository.UpdateAsync(post);
    }

    public async Task On(CommentAddedEvent @event)
    {
        await _commentRepository.CreateAsync(new CommentEntity
        {
            PostId = @event.Id,
            CommentId = @event.CommentId,
            Comment = @event.Comment,
            CreateAt = @event.CreateAt,
            Username = @event.Username,
            Edited = false
        });
    }

    public async Task On(CommentEditedEvent @event)
    {
        var comment = await _commentRepository.GetByIdAsync(@event.CommentId);
        if(comment is null) return;
        comment.Comment = @event.Comment;
        comment.Edited = true;
        comment.CreateAt = @event.UpdateAt;
        await _commentRepository.UpdateAsync(comment);
    }

    public async Task On(CommentRemovedEvent @event)
    {
        await _commentRepository.DeleteAsync(@event.CommentId);
    }

    public async Task On(PostRemovedEvent @event)
    {
        await _postRepository.DeleteAsync(@event.Id);
    }
}