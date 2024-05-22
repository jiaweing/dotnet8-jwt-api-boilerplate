namespace Api.Application.Models.OneOf;

public class Success<T>
{
    public T Result { get; set; }

    public Success()
    {

    }
    public Success(T result)
    {
        Result = result;
    }
}
