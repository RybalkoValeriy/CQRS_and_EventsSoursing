using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories;

public class PostRepository(DatabaseContextFactory contextFactory) : IPostRepository
{
    public async Task<Guid> CreateAsync(Domain.Entities.Post post)
    {
        await using var context = contextFactory.CreateDbContext();

        context.Posts.Add(post);

        await context.SaveChangesAsync();

        return post.Id;
    }

    public async Task DeleteAsync(Guid postId)
    {
        await using var context = contextFactory.CreateDbContext();

        var post = await GetByIdAsync(postId);

        context.Posts.Remove(post);

        await context.SaveChangesAsync();
    }

    public async Task<List<Domain.Entities.Post>> ListByAuthorAsync(string author)
    {
        await using var context = contextFactory.CreateDbContext();

        return await context.Posts.AsNoTracking()
            .Include(i => i.Comments)
            .Where(x => x.Author.Contains(author))
            .ToListAsync();
    }

    public async Task<Domain.Entities.Post> GetByIdAsync(Guid postId)
    {
        await using var context = contextFactory.CreateDbContext();

        var post = await context.Posts
            .AsNoTracking()
            .Include(i => i.Comments)
            .FirstOrDefaultAsync(x => x.Id == postId);

        return post ?? throw new ArgumentException("Post is not found");
    }

    public async Task<List<Domain.Entities.Post>> ListAllAsync()
    {
        await using var context = contextFactory.CreateDbContext();

        return await context.Posts
            .Include(i => i.Comments)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Domain.Entities.Post>> ListWithCommentsAsync()
    {
        await using var context = contextFactory.CreateDbContext();
        return await context.Posts
            .Include(i => i.Comments)
            .AsNoTracking()
            .Where(x => x.Comments.Count > 0)
            .ToListAsync();
    }

    public async Task<List<Domain.Entities.Post>> ListWithLikesAsync(int numberOfLikes)
    {
        await using var context = contextFactory.CreateDbContext();

        return await context.Posts.AsNoTracking()
            .Include(i => i.Comments)
            .AsNoTracking()
            .Where(x => x.Likes >= numberOfLikes)
            .ToListAsync();
    }

    public async Task UpdateAsync(Domain.Entities.Post post)
    {
        await using var context = contextFactory.CreateDbContext();

        context.Posts.Update(post);

        await context.SaveChangesAsync();
    }
}