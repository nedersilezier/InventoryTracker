namespace InventoryTracker.Contracts.Helpers
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }
        public Dictionary<string, string[]>? ValidationErrors { get; set; }
        public int? StatusCode { get; set; }

        public static ServiceResult<T> Ok(T data)
        {
            return new ServiceResult<T>
            {
                Success = true,
                Data = data
            };
        }

        public static ServiceResult<T> Fail(string? errorMessage, Dictionary<string, string[]>? validationErrors = null, int? statusCode = null)
        {
            return new ServiceResult<T>
            {
                Success = false,
                ErrorMessage = errorMessage,
                StatusCode = statusCode,
                ValidationErrors = validationErrors
            };
        }
    }
    public class ServiceResult : ServiceResult<object>
    {
        public static ServiceResult Ok() => new()
        {
            Success = true
        };

        public new static ServiceResult Fail(
            string? errorMessage,
            Dictionary<string, string[]>? validationErrors = null,
            int? statusCode = null) => new()
            {
                Success = false,
                ErrorMessage = errorMessage,
                ValidationErrors = validationErrors,
                StatusCode = statusCode
            };
    }
}
