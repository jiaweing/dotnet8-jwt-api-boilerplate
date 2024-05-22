namespace Api.Application.Models.OneOf;

public class Forbidden
{
    public string Message { get; set; }

    public Forbidden()
    {

    }

    public Forbidden(string message)
    {
        Message = message;
    }
}
