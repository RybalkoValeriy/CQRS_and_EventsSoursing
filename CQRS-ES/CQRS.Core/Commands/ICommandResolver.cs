using CQRS.Core.Handlers;

namespace CQRS.Core.Commands;
public interface ICommandResolver
{
    ICommandHandler<ICommand> ResolveFor<ICommand>() where ICommand : BaseCommand;
}
