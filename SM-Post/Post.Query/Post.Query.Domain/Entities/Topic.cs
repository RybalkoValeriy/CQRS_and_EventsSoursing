using CQRS.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Post.Query.Domain.Entities;

[Table("Topic", Schema = "dbo")]
public class Topic : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Article> Articles { get; set; }
    public Guid UserId { get; set; }
    public User Author { get; set; }
}