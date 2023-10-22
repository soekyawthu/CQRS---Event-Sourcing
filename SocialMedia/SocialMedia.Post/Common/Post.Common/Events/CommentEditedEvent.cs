using SocialMedia.Core.Events;

namespace Post.Common.Events;

public class CommentEditedEvent : BaseEvent
{
    protected CommentEditedEvent() : base(nameof(CommentEditedEvent))
    {
    }
    
    public Guid CommentId { get; set; }
    public required string Comment { get; set; }
    public required string Username { get; set; }
    public DateTime UpdateAt { get; set; }
}