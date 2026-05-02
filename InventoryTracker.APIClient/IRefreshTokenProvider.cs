using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.APIClient
{
    public interface IRefreshTokenProvider
    {
        string? GetRefreshToken();
    }
}
