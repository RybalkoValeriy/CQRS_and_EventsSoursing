using CQRS.Core.Handlers;
using Post.Query.Domain.Repositories;

namespace Post.Cmd.Api.Commands.Topic
{
    public class NewTopicCommandHandler(ITopicRepository topicRepository) : ICommandHandler<NewTopicCommand>
    {
        public async Task SendAsync(NewTopicCommand command, CancellationToken cancellationToken = default)
        {
            var topic = new Query.Domain.Entities.Topic
            {
                Id = command.Id,
                Name = command.Name,
                UserId = command.AuthorId
            };

            await topicRepository.CreateAsync(topic, cancellationToken);
        }
    }
}
