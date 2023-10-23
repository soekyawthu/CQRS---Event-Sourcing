using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Infrastructure.Repositories;

public class PostRepository : IPostRepository
{
    private readonly PostDbContext _dbContext;

    public PostRepository(PostDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task CreateAsync(PostEntity post)
    {
        _dbContext.Posts!.Add(post);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(PostEntity post)
    {
        _dbContext.Posts!.Update(post);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid postId)
    {
        var post = await GetByIdAsync(postId);
        
        if(post is null) return;

        _dbContext.Posts!.Remove(post);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<PostEntity?> GetByIdAsync(Guid postId)
    {
        return await _dbContext.Posts!.AsNoTracking().FirstOrDefaultAsync(x => x.PostId == postId);
    }

    public async Task<List<PostEntity>> ListAllAsync()
    {
        return await _dbContext.Posts!
            .AsNoTracking()
            .Include(x => x.Comments)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<PostEntity>> ListByAuthorAsync(string author)
    {
        return await _dbContext.Posts!.AsNoTracking()
            .Where(x => x.Author == author)
            .Include(x => x.Comments)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<PostEntity>> ListWithLikesAsync(int numberOfLikes)
    {
        return await _dbContext.Posts!.AsNoTracking()
            .Include(x => x.Comments)
            .AsNoTracking()
            .Where(x => x.Likes >= numberOfLikes)
            .ToListAsync();
    }

    public async Task<List<PostEntity>> ListWithCommentsAsync()
    {
        return await _dbContext.Posts!.AsNoTracking()
            .Include(i => i.Comments).AsNoTracking()
            .Where(x => x.Comments != null && x.Comments.Any())
            .ToListAsync();
    }
}