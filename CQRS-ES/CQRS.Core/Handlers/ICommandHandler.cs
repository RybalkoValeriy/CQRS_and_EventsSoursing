using CQRS.Core.Commands;

namespace CQRS.Core.Handlers;

public interface ICommandHandler<in ICommand> where ICommand : BaseCommand
{
    Task SendAsync(ICommand command, CancellationToken cancellationToken = default);
}