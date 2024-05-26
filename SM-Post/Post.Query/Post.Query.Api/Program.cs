using Confluent.Kafka;
using CQRS.Core.Consumers;
using CQRS.Core.Handlers;
using CQRS.Core.Queries;
using Microsoft.EntityFrameworkCore;
using Post.Query.Api.Queries;
using Post.Query.Api.Queries.Topics;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure;
using Post.Query.Infrastructure.Consumers;
using Post.Query.Infrastructure.DataAccess;
using Post.Query.Infrastructure.Handlers;
using Post.Query.Infrastructure.Repositories;
using Post.Query.Infrastructure.Resolver;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddRouting()
    .AddQueryServices(builder.Configuration)
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddControllers();

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
builder.Services.AddSingleton<IEventHandlerOld, EventHandlerOld>();
builder.Services.AddScoped(typeof(IQueryResolver), typeof(QueryResolver));

// register query handler methods
// Posts
//var postQueryHandler = builder.Services.BuildServiceProvider().GetRequiredService<IPostQueryHandler>();
//var postQueryDispatcher = new QueryDispatcher<Post.Query.Domain.Entities.Post>()
//    .RegisterHandler<FindAllPostsQuery>(postQueryHandler.HandleAsync)
//    .RegisterHandler<FindPostByIdQuery>(postQueryHandler.HandleAsync)
//    .RegisterHandler<FindPostsByAuthorQuery>(postQueryHandler.HandleAsync)
//    .RegisterHandler<FindPostsWithCommentsQuery>(postQueryHandler.HandleAsync)
//    .RegisterHandler<FindPostsWithLikesQuery>(postQueryHandler.HandleAsync);
//builder.Services.AddScoped(_ => postQueryDispatcher);

// Topics
builder.Services.AddScoped<IQueryHandler<GetAllTopicsQuery, List<Topic>>, GetAllTopicsQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetTopicByIdQuery, Topic>, GetTopicByIdQueryHandler>();

builder.Services.AddHostedService<ConsumerHostedService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsEnvironment("Development.PostgreSQL"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();