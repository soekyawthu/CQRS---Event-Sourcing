using Post.Common.Events;
using SocialMedia.Core.Domain;

namespace Post.Command.Domain;

public class PostAggregate : AggregateRoot
{
    private bool _active;
    private string? _author;
    private readonly Dictionary<Guid, Tuple<string, string>> _comments = new();

    public bool Active
    {
        get => _active; set => _active = value;
    }

    public PostAggregate()
    {
    }
  
    /* Create Post */
    public PostAggregate(Guid id, string author, string text)
    {
        RaiseEvent(new PostCreatedEvent
        {
            Id = id,
            Author = author,
            Text = text,
            CreateAt = DateTime.Now
        });
    }
    
    public void Apply(PostCreatedEvent @event)
    {
        _id = @event.Id;
        _active = true;
        _author = @event.Author;
    }
    
    /* Edit Post */
    public void EditPost(string text)
    {
        if (!_active)
        {
            throw new InvalidOperationException("You cannot edit the message of an inactive post!");
        }

        if (string.IsNullOrWhiteSpace(text))
        {
            throw new InvalidOperationException($"The value of {nameof(text)} cannot be null or empty. Please provide a valid {nameof(text)}!");
        }

        RaiseEvent(new PostEditedEvent
        {
            Id = _id,
            Text = text
        });
    }

    public void Apply(PostEditedEvent @event)
    {
        _id = @event.Id;
    }
    
    /* Delete Post */
    public void DeletePost(string username)
    {
        if (!_active)
        {
            throw new InvalidOperationException("The post has already been removed!");
        }

        if (!_author!.Equals(username, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new InvalidOperationException("You are not allowed to delete a post that was made by someone else!");
        }

        RaiseEvent(new PostRemovedEvent
        {
            Id = _id
        });
    }

    public void Apply(PostRemovedEvent @event)
    {
        _id = @event.Id;
        _active = false;
    }

    /* Like Post */
    public void LikePost()
    {
        if (!_active)
        {
            throw new InvalidOperationException("You cannot like an inactive post!");
        }

        RaiseEvent(new PostLikedEvent
        {
            Id = _id
        });
    }

    public void Apply(PostLikedEvent @event)
    {
        _id = @event.Id;
    }
    
    /* Add Comment */
    public void AddComment(string comment, string username)
    {
        if (!_active)
        {
            throw new InvalidOperationException("You cannot add a comment to an inactive post!");
        }

        if (string.IsNullOrWhiteSpace(comment))
        {
            throw new InvalidOperationException($"The value of {nameof(comment)} cannot be null or empty. Please provide a valid {nameof(comment)}!");
        }

        RaiseEvent(new CommentAddedEvent
        {
            Id = _id,
            CommentId = Guid.NewGuid(),
            Comment = comment,
            Username = username,
            CreateAt = DateTime.Now
        });
    }

    public void Apply(CommentAddedEvent @event)
    {
        _id = @event.Id;
        _comments.Add(@event.CommentId, new Tuple<string, string>(@event.Comment, @event.Username));
    }
    
    /* Edit Comment */
    public void EditComment(Guid commentId, string comment, string username)
    {
        if (!_active)
        {
            throw new InvalidOperationException("You cannot edit a comment of an inactive post!");
        }

        if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new InvalidOperationException("You are not allowed to edit a comment that was made by another user!");
        }

        RaiseEvent(new CommentEditedEvent
        {
            Id = _id,
            CommentId = commentId,
            Comment = comment,
            Username = username,
            UpdateAt = DateTime.Now
        });
    }

    public void Apply(CommentEditedEvent @event)
    {
        _id = @event.Id;
        _comments[@event.CommentId] = new Tuple<string, string>(@event.Comment, @event.Username);
    }
    
    /* Remove Comment */
    public void RemoveComment(Guid commentId, string username)
    {
        if (!_active)
        {
            throw new InvalidOperationException("You cannot remove a comment of an inactive post!");
        }

        if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new InvalidOperationException("You are not allowed to remove a comment that was made by another user!");
        }

        RaiseEvent(new CommentRemovedEvent
        {
            Id = _id,
            CommentId = commentId
        });
    }

    public void Apply(CommentRemovedEvent @event)
    {
        _id = @event.Id;
        _comments.Remove(@event.CommentId);
    }

}