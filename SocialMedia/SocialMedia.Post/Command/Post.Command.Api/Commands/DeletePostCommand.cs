using SocialMedia.Core.Commands;

namespace Post.Command.Api.Commands;

public class DeletePostCommand : BaseCommand
{
    public required string Username { get; set; }
}