using System.ComponentModel.DataAnnotations.Schema;
using CQRS.Core.Domain;

namespace Post.Query.Domain.Entities;

[Table("Article", Schema = "dbo")]
public class Article : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Body { get; set; }
    public Guid TopicId { get; set; }
    public Topic Topic { get; set; }
    public Guid UserId { get; set; }
    public User Author { get; set; }
}