using InventoryTracker.Contracts.Helpers;
using System.Net.Http.Json;

namespace InventoryTracker.APIClient
{
    public class ApiHttpClient
    {
        private readonly HttpClient _httpClient;

        public ApiHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ServiceResult<T>> SendAsync<T>(HttpMethod method, string url, object? body, string fallbackMessage, CancellationToken cancellationToken)
        {
            using var request = new HttpRequestMessage(method, url);

            if (body is not null)
                request.Content = JsonContent.Create(body);

            using var response = await _httpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return await ApiErrorParser.ToFailResult<T>(response, fallbackMessage, cancellationToken);

            var content = await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);

            if (content is null)
                return ServiceResult<T>.Fail("Failed to parse response from server.", statusCode: (int)response.StatusCode);

            return ServiceResult<T>.Ok(content);
        }

        public Task<ServiceResult<T>> GetAsync<T>(string url, string fallbackMessage, CancellationToken cancellationToken)
        {
            return SendAsync<T>(HttpMethod.Get, url, null, fallbackMessage, cancellationToken);
        }

        public Task<ServiceResult<T>> PostAsync<T>(string url, object body, string fallbackMessage, CancellationToken cancellationToken)
        {
            return SendAsync<T>(HttpMethod.Post, url, body, fallbackMessage, cancellationToken);
        }

        public Task<ServiceResult<T>> PutAsync<T>(string url, object body, string fallbackMessage, CancellationToken cancellationToken)
        {
            return SendAsync<T>(HttpMethod.Put, url, body, fallbackMessage, cancellationToken);
        }

        public Task<ServiceResult<T>> PatchAsync<T>(string url, object? body, string fallbackMessage, CancellationToken cancellationToken)
        {
            return SendAsync<T>(HttpMethod.Patch, url, body ?? new { }, fallbackMessage, cancellationToken);
        }
    }
}