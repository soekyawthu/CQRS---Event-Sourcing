using SocialMedia.Core.Events;

namespace Post.Common.Events;

public class PostLikedEvent : BaseEvent
{
    protected PostLikedEvent() : base(nameof(PostLikedEvent))
    {
    }
}