using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Post.Query.Domain.Entities;
using Post.Query.Infrastructure.Repositories;

namespace Post.Query.Infrastructure.Test;

public class PostRepositoryTest
{
    [Fact]
    public async Task Should_Save_PostEntity()
    {
        /*const string connectionString =
            "Server=localhost,1433;Database=SocialMedia;User Id=sa;Password=skt;Encrypt=False;Trusted_Connection=True;";
        var option = new DbContextOptionsBuilder<PostDbContext>()
            .UseSqlServer(connectionString)
            .Options;*/

        var dbContextFactory = new PostDbContextFactory(ConfigureDbContext);
        await using var dbContext = dbContextFactory.CreateDbContext();
        var loggerMock = new Mock<ILogger<PostRepository>>();
        
        var id = Guid.NewGuid();
        var date = DateTime.Now;
        var entity = new PostEntity
        {
            PostId = id,
            Author = "Soe Kyaw Thu",
            Text = "CQRS",
            CreateAt = date
        };

        var repository = new PostRepository(loggerMock.Object, dbContextFactory);
        Assert.NotNull(repository);
        await repository.CreateAsync(entity);
        var result = await dbContext.Posts!.FirstOrDefaultAsync(x => x.PostId == id);
        Assert.NotNull(result);
        Assert.Equal(entity.PostId, result.PostId);
        Assert.Equal(entity.Author, result.Author);
        Assert.Equal(entity.Text, result.Text);
        Assert.Equal(entity.CreateAt, result.CreateAt);
        return;

        void ConfigureDbContext(DbContextOptionsBuilder builder) => builder.UseInMemoryDatabase("SocialMedia");
    }
}