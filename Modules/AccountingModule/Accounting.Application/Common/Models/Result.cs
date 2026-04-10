namespace Accounting.Application.Common.Models
{
    public interface IResult
    {
        bool Success { get; set; }
        bool HasWarning { get; set; }
        string Message { get; set; }
    }


    public class Result : IResult
    {
        public bool Success { get; set; }
        public bool HasWarning { get; set; }
        public string Message { get; set; }


        public Result() {}

        public Result(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public static Result Ok(string message = "") => new Result(true, message);
        public static Result SuccessWithWarning(string message) => new Result(true, message) { HasWarning = true };
        public static Result Failure(string message) => new Result(false, message);
        public static implicit operator Result(MediatR.Unit _) => new Result(true, string.Empty);
    }


    public class Result<T> : Result
    {
        public T? Data { get; set; }

        public Result() {}

        public Result(bool success, string message, T? data) : base(success, message)
        {
            Data = data;
        }

        public static Result<T> Ok(T data, string message = "") => new Result<T>(true, message, data);
        public static Result<T> SuccessWithWarning(T data, string message) => new Result<T>(true, message, data) { HasWarning = true };
        public new static Result<T> Failure(string message) => new Result<T>(false, message, default);

        public static implicit operator Result<T>(T data) => new Result<T>(true, string.Empty, data);
    }
}

