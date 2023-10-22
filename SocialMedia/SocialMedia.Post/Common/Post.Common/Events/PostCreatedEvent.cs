using SocialMedia.Core.Events;

namespace Post.Common.Events;

public class PostCreatedEvent : BaseEvent
{
    protected PostCreatedEvent() : base(nameof(PostCreatedEvent))
    {
    }
    
    public required string Author { get; set; }
    public required string Text { get; set; }
    public DateTime CreateAt { get; set; }
}