namespace Api.Application.Models.OneOf;

public class Error
{
    public string Message { get; }

    public Error()
    {

    }

    public Error(string errorMessage)
    {
        Message = errorMessage;
    }
}