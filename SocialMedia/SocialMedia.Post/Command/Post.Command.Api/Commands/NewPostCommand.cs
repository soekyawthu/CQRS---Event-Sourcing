using SocialMedia.Core.Commands;

namespace Post.Command.Api.Commands;

public class NewPostCommand : BaseCommand
{
    public required string Author { get; set; }
    public required string Text { get; set; }
}