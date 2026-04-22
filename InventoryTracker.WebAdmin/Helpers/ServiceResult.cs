namespace InventoryTracker.WebAdmin.Helpers
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }
        public Dictionary<string, string[]>? ValidationErrors { get; set; }

        public static ServiceResult<T> Ok(T data)
        {
            return new ServiceResult<T>
            {
                Success = true,
                Data = data
            };
        }

        public static ServiceResult<T> Fail(string? errorMessage, Dictionary<string, string[]>? validationErrors = null)
        {
            return new ServiceResult<T>
            {
                Success = false,
                ErrorMessage = errorMessage,
                ValidationErrors = validationErrors
            };
        }
    }
}
