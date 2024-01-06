using Confluent.Kafka;
using CQRS.Core.Consumers;
using CQRS.Core.Handlers;
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
using Post.Query.Infrastructure.Resolver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add services to the container.
Action<DbContextOptionsBuilder> configDbContext;
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

if (env.Equals("Development.PostgreSQL"))
{
    configDbContext = (
        o => o.UseNpgsql(builder.Configuration.GetConnectionString("SqlServer")));
}
else
{
    configDbContext = (
        o => o.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
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
builder.Services.AddSingleton<IEventHandlerOld, EventHandlerOld>();
builder.Services.AddScoped(typeof(IQueryResolver), typeof(QueryResolver));

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
// builder.Services.AddScoped<IQueryHandler<Topic, TopicCreateEvent>, TopicCreatedEventHandler>();
builder.Services.AddScoped<IQueryHandler<GetAllTopicsQuery, List<Topic>>, GetAllTopicsQueryHandler>();

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