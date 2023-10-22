using SocialMedia.Core.Commands;

namespace Post.Command.Api.Commands;

public class EditCommentCommand : BaseCommand
{
    public required Guid CommentId { get; set; }
    public required string Comment { get; set; }
    public required string Username { get; set; }
}