using System.Diagnostics.CodeAnalysis;

namespace Post.Query.Domain.Entities;

public record User
{
    public required Id Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Login { get; init; }
    public required string Password { get; init; }
    public virtual ICollection<Topic> Topics { get; init; } = new List<Topic>();
    public virtual ICollection<Article> Articles { get; init; } = new List<Article>();

    [SetsRequiredMembers]
    public User(Id id, string firstName, string lastName, string login, string password)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Login = login;
        Password = password;
    }
}