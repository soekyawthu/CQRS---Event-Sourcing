using SocialMedia.Core.Events;

namespace Post.Common.Events;

public class CommentAddedEvent : BaseEvent
{
    protected CommentAddedEvent() : base(nameof(CommentAddedEvent))
    {
    }
    
    public required string CommentId { get; set; }
    public required string Comment { get; set; }
    public required string Username { get; set; }
    public DateTime CreateAt { get; set; }
}