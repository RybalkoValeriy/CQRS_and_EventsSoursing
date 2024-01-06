using CQRS.Core.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Post.Query.Domain.Entities;

[Table("Post", Schema = "dbo")]
public class Post : IEntity<Guid>
{
    [Key] 
    public Guid Id { get; set; }
    public string Author { get; set; }
    public DateTime DatePosted { get; set; }
    public string Message { get; set; }
    public int Likes { get; set; }
    public ICollection<Comment> Comments { get; set; }
}