namespace Api.Application.Models.OneOf;

public class NotFound
{
    public string Message { get; set; }

    public NotFound()
    {

    }

    public NotFound(string message)
    {
        Message = message;
    }
}
