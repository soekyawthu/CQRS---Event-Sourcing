using SocialMedia.Core.Commands;

namespace Post.Command.Api.Commands;

public class AddCommentCommand : BaseCommand
{
    public required string Comment { get; set; }
    public required string Username { get; set; }
}