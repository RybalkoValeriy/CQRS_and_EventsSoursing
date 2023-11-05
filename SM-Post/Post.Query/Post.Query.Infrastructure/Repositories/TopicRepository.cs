using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories;

public class TopicRepository : ITopicRepository
{
    private readonly DatabaseContextFactory _contextFactory;

    public TopicRepository(DatabaseContextFactory contextFactory) =>
        _contextFactory = contextFactory;

    public async Task<List<Topic>> GetAllAsync()
    {
        await using var context = _contextFactory.CreateDbContext();

        return await context.Topics.AsNoTracking().ToListAsync();
    }
}