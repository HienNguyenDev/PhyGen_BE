namespace PhyGen.Application.Common
{
    public class ApiResult<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        // Success with data and optional message
        public static ApiResult<T> Success(T data, string message)
        {
            return new ApiResult<T>
            {
                IsSuccess = true,
                Data = data,
                Message = message
            };
        }

        // Success without data
        public static ApiResult<T> SuccessMessage(string message)
        {
            return new ApiResult<T>
            {
                IsSuccess = true,
                Message = message
            };
        }

        // Failure result
        public static ApiResult<T> Failure(string message)
        {
            return new ApiResult<T>
            {
                IsSuccess = false,
                Message = message,
                Data = default
            };
        }
    }
}
