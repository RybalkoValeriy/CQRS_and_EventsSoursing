namespace Post.Query.Api.Queries;

public interface IPostQueryHandler
{
    Task<List<Domain.Entities.Post>> HandleAsync(FindAllPostsQuery query);
    Task<List<Domain.Entities.Post>> HandleAsync(FindPostByIdQuery query);
    Task<List<Domain.Entities.Post>> HandleAsync(FindPostsByAuthorQuery query);
    Task<List<Domain.Entities.Post>> HandleAsync(FindPostsWithCommentsQuery query);
    Task<List<Domain.Entities.Post>> HandleAsync(FindPostsWithLikesQuery query);
}