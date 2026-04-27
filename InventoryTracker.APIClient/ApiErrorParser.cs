using InventoryTracker.Contracts.Helpers;
using System.Text.Json;

namespace InventoryTracker.APIClient
{
    public static class ApiErrorParser
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public static async Task<ServiceResult<T>> ToFailResult<T>(HttpResponseMessage response, string fallbackMessage, CancellationToken cancellationToken)
        {
            var raw = await response.Content.ReadAsStringAsync(cancellationToken);

            ApiProblemDetails? problem = null;

            if (!string.IsNullOrWhiteSpace(raw))
            {
                try
                {
                    problem = JsonSerializer.Deserialize<ApiProblemDetails>(raw, JsonOptions);
                }
                catch (JsonException)
                {
                }
            }
            return ServiceResult<T>.Fail(
                problem?.Detail ?? problem?.Title ?? fallbackMessage,
                problem?.Errors,
                problem?.Status ?? (int)response.StatusCode);
        }
    }
}
