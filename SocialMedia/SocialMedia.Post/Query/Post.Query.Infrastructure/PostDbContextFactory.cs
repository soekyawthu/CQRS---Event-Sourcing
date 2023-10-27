using Microsoft.EntityFrameworkCore;

namespace Post.Query.Infrastructure;

public class PostDbContextFactory
{
    private readonly Action<DbContextOptionsBuilder> _configureDbContext;

    public PostDbContextFactory(Action<DbContextOptionsBuilder> configureDbContext)
    {
        _configureDbContext = configureDbContext;
    }

    public PostDbContext CreateDbContext()
    {
        DbContextOptionsBuilder<PostDbContext> optionsBuilder = new();
        _configureDbContext(optionsBuilder);
        return new PostDbContext(optionsBuilder.Options);
    }
}