using Confluent.Kafka;
using CQRS.Core.Consumers;
using CQRS.Core.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Post.Query.Api.Queries;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.Consumers;
using Post.Query.Infrastructure.DataAccess;
using Post.Query.Infrastructure.Dispatchers;
using Post.Query.Infrastructure.Handlers;
using Post.Query.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Action<DbContextOptionsBuilder> configDbContext;
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

if (env.Equals("Development.PostgreSQL"))
{
    configDbContext = (
        o => o.UseLazyLoadingProxies(false).UseNpgsql(builder.Configuration.GetConnectionString("SqlServer")));
}
else
{
    configDbContext = (
        o => o.UseLazyLoadingProxies(false).UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
}

// Add services to the container.

builder.Services.AddDbContext<DatabaseContext>(configDbContext);
builder.Services.AddSingleton(new DatabaseContextFactory(configDbContext));


// create db table from code
DatabaseContext dbContext;

dbContext = builder.Services.BuildServiceProvider().GetRequiredService<DatabaseContext>();

dbContext.Database.EnsureCreated();

builder.Services.AddSingleton<IPostRepository, PostRepository>();
builder.Services.AddSingleton<ITopicRepository, TopicRepository>();
builder.Services.AddSingleton<ICommentRepository, CommentRepository>();
builder.Services.AddSingleton<IPostQueryHandler, PostQueryHandler>();
builder.Services.AddSingleton<ITopicQueryHandler, TopicQueryHandler>();

builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection(nameof(ConsumerConfig)));
builder.Services.AddScoped<IEventConsumer, EventConsumer>();
builder.Services.AddSingleton<IEventHandler, Post.Query.Infrastructure.Handlers.EventHandler>();

// register query handler methods
// Post
var postQueryHandler = builder.Services.BuildServiceProvider().GetRequiredService<IPostQueryHandler>();
var postQueryDispatcher = new QueryDispatcher<Post.Query.Domain.Entities.Post>()
    .RegisterHandler<FindAllPostsQuery>(postQueryHandler.HandleAsync)
    .RegisterHandler<FindPostByIdQuery>(postQueryHandler.HandleAsync)
    .RegisterHandler<FindPostsByAuthorQuery>(postQueryHandler.HandleAsync)
    .RegisterHandler<FindPostsWithCommentsQuery>(postQueryHandler.HandleAsync)
    .RegisterHandler<FindPostsWithLikesQuery>(postQueryHandler.HandleAsync);
builder.Services.AddScoped(_ => postQueryDispatcher);

// Topic
var topicQueryHandler = builder.Services.BuildServiceProvider().GetRequiredService<ITopicQueryHandler>();
var topicQueryDispatcher = new QueryDispatcher<Topic>()
    .RegisterHandler<GetAllTopicsQuery>(topicQueryHandler.HandleAsync);
builder.Services.AddScoped(_ => topicQueryDispatcher);

builder.Services.AddControllers();
builder.Services.AddHostedService<ConsumerHostedService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();