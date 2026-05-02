using InventoryTracker.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.APIClient
{
    public interface ITokenStore
    {
        void StoreTokens(AuthResponseDTO tokens);
        void ClearTokens();
    }
}
