using SocialMedia.Core.Events;

namespace Post.Common.Events;

public class PostEditedEvent : BaseEvent
{
    protected PostEditedEvent() : base(nameof(PostEditedEvent))
    {
    }
    
    public required string Text { get; set; }
}