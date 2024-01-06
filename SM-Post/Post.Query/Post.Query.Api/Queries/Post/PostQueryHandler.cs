using Post.Query.Domain.Repositories;

namespace Post.Query.Api.Queries;

public class PostQueryHandler : IPostQueryHandler
{
    private readonly IPostRepository _postRepository;

    public PostQueryHandler(IPostRepository postRepository) =>
        _postRepository = postRepository;

    public Task<List<Domain.Entities.Post>> HandleAsync(FindAllPostsQuery query)
        => _postRepository.ListAllAsync();

    public async Task<List<Domain.Entities.Post>> HandleAsync(FindPostByIdQuery query)
    {
        var post = await _postRepository.GetByIdAsync(query.Id);
        return new List<Domain.Entities.Post> { post };
    }

    public Task<List<Domain.Entities.Post>> HandleAsync(FindPostsByAuthorQuery query) =>
        _postRepository.ListByAuthorAsync(query.Author);

    public Task<List<Domain.Entities.Post>> HandleAsync(FindPostsWithCommentsQuery query) =>
        _postRepository.ListWithCommentsAsync();

    public Task<List<Domain.Entities.Post>> HandleAsync(FindPostsWithLikesQuery query) =>
        _postRepository.ListWithLikesAsync(query.NumberOfLikes);
}