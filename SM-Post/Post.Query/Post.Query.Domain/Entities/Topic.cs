using System.Diagnostics.CodeAnalysis;

namespace Post.Query.Domain.Entities;

public record Topic
{
    public required Id Id { get; init; }

    public required string Name { get; init; }

    public required Id UserId { get; init; }

    public User? Author { get; init; }

    public virtual ICollection<Article> Articles { get; init; } = new List<Article>();

    [SetsRequiredMembers]
    public Topic(Id id, string name, Id userId)
    {
        Id = id;
        Name = name;
        UserId = userId;
    }
}
