namespace Api.Application.Models.OneOf;

public class Updated
{
    public string Message { get; set; }

    public Updated()
    {

    }

    public Updated(string message)
    {
        Message = message;
    }
}
