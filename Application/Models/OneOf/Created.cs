namespace Api.Application.Models.OneOf;

public class Created<T>
{
    public T Value { get; set; }

    public Created()
    {

    }

    public Created(T value)
    {
        Value = value;
    }
}