using InventoryTracker.Contracts.Requests.Auth;
using InventoryTracker.Contracts.Responses;
using InventoryTracker.WebAdmin.Exceptions;
using InventoryTracker.WebAdmin.Interfaces;
using InventoryTracker.WebAdmin.Models;
using InventoryTracker.WebAdmin.ViewModels.Login;
using System.Text.Json;

namespace InventoryTracker.WebAdmin.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<AuthResponseDTO?> LoginAsync(LoginViewModel request, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/login", request, cancellationToken);
            if(response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AuthResponseDTO>(cancellationToken: cancellationToken);
            }
            await ThrowApiExceptionAsync(response, cancellationToken);
            throw new Exception("Unreachable?");
        }

        public async Task LogoutAsync(LogoutRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/logout", request);
             response.EnsureSuccessStatusCode();
        }
        private static async Task ThrowApiExceptionAsync(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            var raw = await response.Content.ReadAsStringAsync(cancellationToken);

            ApiProblemDetails? problem = null;

            try
            {
                problem = JsonSerializer.Deserialize<ApiProblemDetails>(
                    raw,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
            catch
            {
            }

            if (problem?.Errors is not null && problem.Errors.Count > 0)
            {
                throw new ApiValidationException(
                    problem.Detail ?? problem.Title ?? "Validation failed.",
                    problem.Errors,
                    problem.Status ?? (int)response.StatusCode);
            }

            throw new ApiException(
                problem?.Detail ?? problem?.Title ?? "Request failed.",
                problem?.Status ?? (int)response.StatusCode);
        }
    }
}
