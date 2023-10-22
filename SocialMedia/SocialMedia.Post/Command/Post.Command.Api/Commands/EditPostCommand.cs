using SocialMedia.Core.Commands;

namespace Post.Command.Api.Commands;

public class EditPostCommand : BaseCommand
{
    public required string Text { get; set; }
}