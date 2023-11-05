using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories;

public class PostRepository : IPostRepository
{
    private readonly DatabaseContextFactory _contextFactory;

    public PostRepository(DatabaseContextFactory cf) => _contextFactory = cf;

    public async Task CreateAsync(Domain.Entities.Post post)
    {
        await using var context = _contextFactory.CreateDbContext();

        context.Posts.Add(post);

        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid postId)
    {
        await using var context = _contextFactory.CreateDbContext();

        var post = await GetByIdAsync(postId);

        if (post is null)
        {
            return;
        }

        context.Posts.Remove(post);

        await context.SaveChangesAsync();
    }

    public async Task<List<Domain.Entities.Post>> ListByAuthorAsync(string author)
    {
        await using var context = _contextFactory.CreateDbContext();

        return await context.Posts.AsNoTracking()
            .Include(i => i.Comments).AsNoTracking()
            .Where(x => x.Author.Contains(author))
            .ToListAsync();
    }

    public async Task<Domain.Entities.Post> GetByIdAsync(Guid postId)
    {
        await using var context = _contextFactory.CreateDbContext();
        
        return await context.Posts
            .Include(i => i.Comments)
            .FirstOrDefaultAsync(x => x.Id == postId);
    }

    public async Task<List<Domain.Entities.Post>> ListAllAsync()
    {
        await using var context = _contextFactory.CreateDbContext();
        
        return await context.Posts
            .AsNoTracking()
            .Include(i => i.Comments)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Domain.Entities.Post>> ListWithCommentsAsync()
    {
        await using var context = _contextFactory.CreateDbContext();
        return await context.Posts.AsNoTracking()
            .Include(i => i.Comments)
            .AsNoTracking()
            .Where(x => x.Comments != null && x.Comments.Any())
            .ToListAsync();
    }

    public async Task<List<Domain.Entities.Post>> ListWithLikesAsync(int numberOfLikes)
    {
        await using var context = _contextFactory.CreateDbContext();
        
        return await context.Posts.AsNoTracking()
            .Include(i => i.Comments)
            .AsNoTracking()
            .Where(x => x.Likes >= numberOfLikes)
            .ToListAsync();
    }

    public async Task UpdateAsync(Domain.Entities.Post post)
    {
        await using var context = _contextFactory.CreateDbContext();
        context.Posts.Update(post);

        await context.SaveChangesAsync();
    }
}