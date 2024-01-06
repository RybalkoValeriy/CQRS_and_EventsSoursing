using CQRS.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Post.Query.Domain.Entities;

[Table("Author", Schema = "dbo")]
public class User : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public virtual ICollection<Topic> Topics { get; set; }
    public virtual ICollection<Article> Articles { get; set; }
}