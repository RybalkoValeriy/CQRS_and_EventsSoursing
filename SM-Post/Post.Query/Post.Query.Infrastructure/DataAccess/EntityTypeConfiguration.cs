using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Post.Query.Domain.Entities;

namespace Post.Query.Infrastructure.DataAccess;

public static class Configuration
{
    public static void TopicEntityConfiguration(this ModelBuilder modelBuilder)
    {
        var topicETBuilder = modelBuilder.Entity<Topic>();

        topicETBuilder
            .Property(x => x.Id)
            .HasConversion(
                t => t.Guid,
                p => new Id(p)
            );

        topicETBuilder
            .HasKey(topic => topic.Id);

        topicETBuilder
            .Property(topic => topic.Name);
        topicETBuilder
            .HasOne(topic => topic.Author)
            .WithMany(user => user.Topics)
            .HasForeignKey(topic => topic.UserId);
        topicETBuilder
            .HasMany(topic => topic.Articles)
            .WithOne(article => article.Topic);
    }
}

public class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder
            .HasKey(article => article.Id);
        builder
            .Property(article => article.Name);
        builder
            .Property(article => article.Body);
        builder
            .HasOne(article => article.Topic)
            .WithMany(topic => topic.Articles)
            .HasForeignKey(article => article.TopicId);

        builder
            .HasOne(article => article.Author)
            .WithMany(user => user.Articles)
            .HasForeignKey(article => article.UserId);
    }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasKey(user => user.Id);
        builder
            .Property(user => user.FirstName);
        builder
            .Property(user => user.LastName);
        builder
            .Property(user => user.Login);
        builder
            .Property(user => user.Password);
        builder
            .HasMany(topic => topic.Topics)
            .WithOne(topic => topic.Author)
            .HasForeignKey(topic => topic.UserId);
        builder
            .HasMany(articles => articles.Articles)
            .WithOne(articles => articles.Author)
            .HasForeignKey(a => a.UserId);
    }
}