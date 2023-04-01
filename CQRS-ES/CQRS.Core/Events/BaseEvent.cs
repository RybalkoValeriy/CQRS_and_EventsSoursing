using CQRS.Core.Messages;

namespace CQRS.Core.Events;

public abstract record BaseEvent(string Type, Guid Id) : Message(Id)
{
    public int Version { get; set; }
    public string Type { get; set; } = Type;
}
