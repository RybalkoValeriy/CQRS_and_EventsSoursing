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
        base.OnModelCreating(modelBuilder); 
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured)
        {
            // var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            //
            // if (env is "Development.PostgreSQL")
            // {
                options.UseLazyLoadingProxies(false).UseNpgsql("host=localhost;database=postgres;user id=postgres;password=postgresPsw");
            // }
            // else
            // {
            //     // todo: use ms-sql
            // }
        }
    }
}