using InventoryTracker.Contracts.Requests.Auth;
using InventoryTracker.Contracts.Responses;
using InventoryTracker.Contracts.Helpers;
using System.Net.Http.Json;
using InventoryTracker.APIClient;

namespace InventoryTracker.AuthClient
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ServiceResult<AuthResponseDTO>> LoginAsync(LoginViewModel request, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/login", request, cancellationToken);
            if (!response.IsSuccessStatusCode)
                return await ApiErrorParser.ToFailResult<AuthResponseDTO>(response, "Login failed", cancellationToken);

            var content = await response.Content.ReadFromJsonAsync<AuthResponseDTO>(cancellationToken: cancellationToken);
            if(content == null)
                return ServiceResult<AuthResponseDTO>.Fail("Failed to parse authentication response.", statusCode: (int)response.StatusCode);

            return ServiceResult<AuthResponseDTO>.Ok(content);
        }

        public async Task<ServiceResult> LogoutAsync(LogoutRequest request, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/logout", request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var result = await ApiErrorParser.ToFailResult<object>(response, "Logout failed", cancellationToken);
                return ServiceResult.Fail(result.ErrorMessage, result.ValidationErrors, result.StatusCode);
            }
            return ServiceResult.Ok();
        }
        public async Task<ServiceResult<AuthResponseDTO>> RefreshTokenAsync(TokenRefreshRequest request, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/refresh", request, cancellationToken);
            if (!response.IsSuccessStatusCode)
                return await ApiErrorParser.ToFailResult<AuthResponseDTO>(response, "Refresh token invalid or expired", cancellationToken);

            var content = await response.Content.ReadFromJsonAsync<AuthResponseDTO>(cancellationToken: cancellationToken);
            if (content == null)
                return ServiceResult<AuthResponseDTO>.Fail("Failed to parse authentication response.", statusCode: (int)response.StatusCode);

            return ServiceResult<AuthResponseDTO>.Ok(content);
        }
    }
}
