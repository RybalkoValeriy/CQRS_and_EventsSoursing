namespace Post.Query.Domain.Repositories;

public interface IPostRepository
{
    Task<Guid> CreateAsync(Entities.Post post);
    Task UpdateAsync(Entities.Post post);
    Task DeleteAsync(Guid postId);
    Task<Entities.Post> GetByIdAsync(Guid postId);
    Task<List<Entities.Post>> ListAllAsync();
    Task<List<Entities.Post>> ListByAuthorAsync(string author);
    Task<List<Entities.Post>> ListWithLikesAsync(int likes);
    Task<List<Entities.Post>> ListWithCommentsAsync();
}