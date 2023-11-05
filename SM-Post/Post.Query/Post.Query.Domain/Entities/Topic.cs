using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Post.Query.Domain.Entities;

[Table("Topic", Schema = "dbo")]
public class Topic
{
    [Key] public Guid Id { get; set; }
    public string Name { get; set; }

    public virtual ICollection<Article> Articles { get; set; }
    public virtual User User { get; set; }
}

[Table("User", Schema = "dbo")]
public class User
{
    [Key] public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string Login { get; set; }
    public string Password { get; set; }

    public virtual ICollection<Topic> Topics { get; set; }
    public virtual ICollection<Article> Articles { get; set; }
}

[Table("Article", Schema = "dbo")]
public class Article
{
    [Key] public Guid Id { get; set; }
    public string Name { get; set; }
    public string Body { get; set; }

    public virtual Topic Topic { get; set; }
    public virtual User User { get; set; }
}