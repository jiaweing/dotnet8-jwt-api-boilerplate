namespace Api.Application.Models.Base;

public class BaseResponse<T>
{
    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
    public bool Success { get; set; }
    public T? Result { get; set; }
    public PageInfo PageInfo { get; set; }
    public string Message { get; set; }

    public BaseResponse(T result, long page = 1, long pageSize = 1, long totalPages = 1)
    {
        Success = true;
        Result = result;
        PageInfo = new PageInfo()
        {
            Page = page,
            PageSize = pageSize,
            Total = totalPages
        };
        Message = string.Empty;
    }

    public BaseResponse(string errorMessage)
    {
        Success = false;
        Message = errorMessage;
    }
}

public class PageInfo
{
    public long Page { get; set; }
    public long PageSize { get; set; }
    public long Total { get; set; }
}