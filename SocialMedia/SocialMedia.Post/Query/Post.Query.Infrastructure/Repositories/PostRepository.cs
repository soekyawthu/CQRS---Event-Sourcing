using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Infrastructure.Repositories;

public class PostRepository : IPostRepository
{
    private readonly ILogger<PostRepository> _logger;
    private readonly PostDbContextFactory _dbContextFactory;

    public PostRepository(ILogger<PostRepository> logger, PostDbContextFactory dbContextFactory)
    {
        _logger = logger;
        _dbContextFactory = dbContextFactory;
    }
    
    public async Task CreateAsync(PostEntity post)
    {
        _logger.LogInformation("Invoked CreateAsync method");
        await using var dbContext = _dbContextFactory.CreateDbContext();
        dbContext.Posts!.Add(post);
        _logger.LogInformation("Added post");
        var result = await dbContext.SaveChangesAsync();
        _logger.LogInformation("Save Result => {Result}", result);
    }

    public async Task UpdateAsync(PostEntity post)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        dbContext.Posts!.Update(post);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid postId)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        var post = await GetByIdAsync(postId);
        
        if(post is null) return;

        dbContext.Posts!.Remove(post);
        await dbContext.SaveChangesAsync();
    }

    public async Task<PostEntity?> GetByIdAsync(Guid postId)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        return await dbContext.Posts!.AsNoTracking().FirstOrDefaultAsync(x => x.PostId == postId);
    }

    public async Task<List<PostEntity>> ListAllAsync()
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        return await dbContext.Posts!
            .AsNoTracking()
            .Include(x => x.Comments)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<PostEntity>> ListByAuthorAsync(string author)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        return await dbContext.Posts!.AsNoTracking()
            .Where(x => x.Author == author)
            .Include(x => x.Comments)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<PostEntity>> ListWithLikesAsync(int numberOfLikes)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        return await dbContext.Posts!.AsNoTracking()
            .Include(x => x.Comments)
            .AsNoTracking()
            .Where(x => x.Likes >= numberOfLikes)
            .ToListAsync();
    }

    public async Task<List<PostEntity>> ListWithCommentsAsync()
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        return await dbContext.Posts!.AsNoTracking()
            .Include(i => i.Comments).AsNoTracking()
            .Where(x => x.Comments != null && x.Comments.Any())
            .ToListAsync();
    }
}