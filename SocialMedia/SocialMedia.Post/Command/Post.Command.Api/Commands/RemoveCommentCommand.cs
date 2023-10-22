using SocialMedia.Core.Commands;

namespace Post.Command.Api.Commands;

public class RemoveCommentCommand : BaseCommand
{
    public Guid CommentId { get; set; }
    public required string Username { get; set; }
}