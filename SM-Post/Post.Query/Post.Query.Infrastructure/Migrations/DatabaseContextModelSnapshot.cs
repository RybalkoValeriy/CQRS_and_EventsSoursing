﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Post.Query.Infrastructure.DataAccess;

#nullable disable

namespace Post.Query.Infrastructure.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Post.Query.Domain.Entities.Article", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Body")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid?>("TopicId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("TopicId");

                    b.HasIndex("UserId");

                    b.ToTable("Article", "dbo");
                });

            modelBuilder.Entity("Post.Query.Domain.Entities.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CommentDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CommentText")
                        .HasColumnType("text");

                    b.Property<bool>("Edited")
                        .HasColumnType("boolean");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.ToTable("Comment", "dbo");
                });

            modelBuilder.Entity("Post.Query.Domain.Entities.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Author")
                        .HasColumnType("text");

                    b.Property<DateTime>("DatePosted")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Likes")
                        .HasColumnType("integer");

                    b.Property<string>("Message")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Post", "dbo");
                });

            modelBuilder.Entity("Post.Query.Domain.Entities.Topic", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Topic", "dbo");
                });

            modelBuilder.Entity("Post.Query.Domain.Entities.Author", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("Login")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Author", "dbo");
                });

            modelBuilder.Entity("Post.Query.Domain.Entities.Article", b =>
                {
                    b.HasOne("Post.Query.Domain.Entities.Topic", "Topic")
                        .WithMany("Articles")
                        .HasForeignKey("TopicId");

                    b.HasOne("Post.Query.Domain.Entities.Author", "Author")
                        .WithMany("Articles")
                        .HasForeignKey("UserId");

                    b.Navigation("Topic");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Post.Query.Domain.Entities.Comment", b =>
                {
                    b.HasOne("Post.Query.Domain.Entities.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");
                });

            modelBuilder.Entity("Post.Query.Domain.Entities.Topic", b =>
                {
                    b.HasOne("Post.Query.Domain.Entities.Author", "Author")
                        .WithMany("Topics")
                        .HasForeignKey("UserId");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Post.Query.Domain.Entities.Post", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("Post.Query.Domain.Entities.Topic", b =>
                {
                    b.Navigation("Articles");
                });

            modelBuilder.Entity("Post.Query.Domain.Entities.Author", b =>
                {
                    b.Navigation("Articles");

                    b.Navigation("Topics");
                });
#pragma warning restore 612, 618
        }
    }
}
