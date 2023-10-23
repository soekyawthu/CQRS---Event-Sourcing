using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Infrastructure.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly PostDbContext _dbContext;

    public CommentRepository(PostDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task CreateAsync(CommentEntity comment)
    {
        _dbContext.Comments!.Add(comment);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<CommentEntity?> GetByIdAsync(Guid commentId)
    {
        return await _dbContext.Comments!.AsNoTracking().FirstOrDefaultAsync(x => x.CommentId == commentId);
    }

    public async Task UpdateAsync(CommentEntity comment)
    {
        _dbContext.Comments!.Update(comment);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid commentId)
    {
        var comment = await GetByIdAsync(commentId);
        if(comment is null) return;

        _dbContext.Remove(comment);
        await _dbContext.SaveChangesAsync();
    }
}