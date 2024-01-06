﻿using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories;

public class TopicRepository : ITopicRepository
{
    private readonly DatabaseContextFactory _contextFactory;

    public TopicRepository(DatabaseContextFactory contextFactory) =>
        _contextFactory = contextFactory;

    public async Task<List<Topic>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await using var context = _contextFactory.CreateDbContext();

        return await context
            .Topics
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task CreateAsync(Topic topic, CancellationToken cancellationToken = default)
    {
        await using var context = _contextFactory.CreateDbContext();

        context.Topics.Add(topic);

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Topic> GetTopic(Guid topicId, CancellationToken cancellationToken = default)
    {
        await using var context = _contextFactory.CreateDbContext();

        var topic = await context.Topics.FirstOrDefaultAsync(x => x.Id == topicId, cancellationToken);

        if (topic is null)
        {
            throw new Exception("Current topic is not found");
        }

        return topic;
    }
}