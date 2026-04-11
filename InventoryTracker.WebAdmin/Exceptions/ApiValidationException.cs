namespace InventoryTracker.WebAdmin.Exceptions
{
    public class ApiValidationException : ApiException
    {
        public Dictionary<string, string[]> Errors { get; }

        public ApiValidationException(
            string message,
            Dictionary<string, string[]> errors,
            int statusCode)
            : base(message, statusCode)
        {
            Errors = errors;
        }
    }
}
