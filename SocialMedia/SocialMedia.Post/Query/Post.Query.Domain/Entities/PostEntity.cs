using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Post.Query.Domain.Entities;

[Table("Post")]
public class PostEntity
{
    [Key]
    public Guid PostId { get; set; }
    public required string Author { get; set; }
    public DateTime CreateAt { get; set; }
    public required string Text { get; set; }
    public int Likes { get; set; }
    public virtual ICollection<CommentEntity>? Comments { get; set; }
}