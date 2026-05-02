using InventoryTracker.Contracts.Helpers;
using InventoryTracker.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.APIClient
{
    public interface ITokenRefreshClient
    {
        Task<ServiceResult<AuthResponseDTO>> RefreshAsync(string refreshToken, CancellationToken cancellationToken);
    }
}
