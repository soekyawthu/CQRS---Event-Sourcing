using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;

namespace Post.Query.Infrastructure;

public class PostDbContext : DbContext
{
    public PostDbContext(DbContextOptions<PostDbContext> options) : base(options)
    {
    }
    
    public DbSet<PostEntity>? Posts { get; set; }
    public DbSet<CommentEntity>? Comments { get; set; }
}