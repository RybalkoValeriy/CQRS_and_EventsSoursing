using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;

namespace Post.Query.Infrastructure.DataAccess;

public class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
        // for migrations
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Domain.Entities.Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }

    public DbSet<User> Users { get; set; }
    public DbSet<Topic> Topics { get; set; }
    public DbSet<Article> Articles { get; set; }

    // configure database provider
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // todo: seed update
        // var user1 = new Author
        // {
        //     Id = Guid.NewGuid(),
        //     FirstName = "FirstNameUser1",
        //     LastName = "LastNameUser1",
        //     Login = "LoginUser1",
        //     Password = "PasswordUser1"
        // };
        // var user2 = new Author
        // {
        //     Id = Guid.NewGuid(),
        //     FirstName = "FirstNameUser2",
        //     LastName = "LastNameUser2",
        //     Login = "LoginUser2",
        //     Password = "PasswordUser2"
        // };
        //
        // var topic1 = new Topic
        // {
        //     Id = Guid.NewGuid(),
        //     Name = "Computer science",
        //     UserId = user1.Id,
        //     Author = user1
        // };
        //
        // var topic2 = new Topic
        // {
        //     Id = Guid.NewGuid(),
        //     Name = "Human science",
        //     UserId = user2.Id,
        //     Author = user2,
        // };
        //
        // var article1 = new Article
        // {
        //     Id = Guid.NewGuid(),
        //     Author = user1,
        //     Name = "What is it a Linked list",
        //     Body = "<h1>Linked list</h1><body>Some body text</body>",
        //     TopicId = topic1.Id,
        //     Topic = topic1
        // };
        //
        // var article2 = new Article
        // {
        //     Id = Guid.NewGuid(),
        //     Author = user1,
        //     Name = "What is it a Arrays",
        //     Body = "<h1>Arrays</h1><body>Some body text</body>",
        //     TopicId = topic1.Id,
        //     Topic = topic1,
        // };
        //
        // var article3 = new Article
        // {
        //     Id = Guid.NewGuid(),
        //     Author = user1,
        //     Name = "What is it a Dictionary",
        //     Body = "<h1>Dictionary</h1><body>Some body text</body>",
        //     TopicId = topic1.Id,
        //     Topic = topic1,
        // };
        //
        // var article4 = new Article
        // {
        //     Id = Guid.NewGuid(),
        //     Author = user1,
        //     Name = "What is it a Sorted list",
        //     Body = "<h1>Sorted list</h1><body>Some body text</body>",
        //     TopicId = topic1.Id,
        //     Topic = topic1
        // };
        //
        // var article5 = new Article
        // {
        //     Id = Guid.NewGuid(),
        //     Author = user2,
        //     Name = "What is it a Graph",
        //     Body = "<h1>Graph it is</h1><body>Some body text</body>",
        //     TopicId = topic1.Id,
        //     Topic = topic1
        // };
        //
        // var article6 = new Article
        // {
        //     Id = Guid.NewGuid(),
        //     Author = user2,
        //     Name = "Python functional programming",
        //     Body = "<h1>Python</h1><body>Some body text</body>",
        //     TopicId = topic1.Id,
        //     Topic = topic1
        // };
        //
        // var article7 = new Article
        // {
        //     Id = Guid.NewGuid(),
        //     Author = user2,
        //     Name = "Data types",
        //     Body = "<h1>Data types</h1><body>Some body text</body>",
        //     TopicId = topic1.Id,
        //     Topic = topic1
        // };
        //
        // modelBuilder
        //     .Entity<Author>()
        //     .HasData(user1, user2);
        //
        // modelBuilder
        //     .Entity<Topic>()
        //     .HasData(topic1, topic2);
        //
        // modelBuilder
        //     .Entity<Article>()
        //     .HasData(article1, article2, article3, article4, article5, article6, article7);

        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new TopicConfiguration());
        modelBuilder.ApplyConfiguration(new ArticleConfiguration());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured)
        {
            // var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            //
            // if (env is "Development.PostgreSQL")
            // {
            options.UseNpgsql("host=localhost;database=postgres;user id=postgres;password=postgresPsw");
            // }
            // else
            // {
            //     // todo: use ms-sql
            // }
        }
    }
}