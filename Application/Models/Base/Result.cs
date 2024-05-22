namespace Api.Application.Models.Base
{
    public record Result<T>(ResultType status, T? result, string? message)
    {
        public static Result<T> Success(T result) => new Result<T>(ResultType.Success, result, null);
        public static Result<T> Created(T result) => new Result<T>(ResultType.Created, result, null);
        public static Result<T> Updated(T result) => new Result<T>(ResultType.Updated, result, null);
        public static Result<T> Deleted(T result) => new Result<T>(ResultType.Deleted, result, null);
        public static Result<T> NotFound => new Result<T>(ResultType.NotFound, default, null);
        public static Result<T> BadRequest(string message) => new Result<T>(ResultType.BadRequest, default, message);
        public static Result<T> Forbidden => new Result<T>(ResultType.Forbidden, default, null);
        public static Result<T> Error(string message) => new Result<T>(ResultType.Error, default, message);
    }

    public enum ResultType
    {
        Success,
        Created,
        Updated,
        Deleted,
        NotFound,
        BadRequest,
        Forbidden,
        Error
    }
}
