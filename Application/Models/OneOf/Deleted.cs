namespace Api.Application.Models.OneOf;

public class Deleted
{
    public string Message { get; set; }

    public Deleted()
    {

    }

    public Deleted(string message)
    {
        Message = message;
    }
}
