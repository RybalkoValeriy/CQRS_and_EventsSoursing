namespace Post.Query.Domain.Entities;

public record Id(Guid? Guid) : RecordValidate
{
    private static readonly Action<Guid?>? validate = (Guid? id) =>
    { _ = id ?? throw new ArgumentNullException(nameof(Guid)); };

    protected override void Validate() => validate?.Invoke(Guid);
}
