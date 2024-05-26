namespace Post.Query.Domain.Entities;

public abstract record RecordValidate
{
    protected RecordValidate()
    {
        Validate();
    }

    protected abstract void Validate();
}
