namespace Api.Application.Models.OneOf;

public class BadRequest
{
    public string Message { get; set; }

    public BadRequest()
    {

    }

    public BadRequest(string message)
    {
        Message = message;
    }
}
