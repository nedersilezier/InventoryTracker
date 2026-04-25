using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.APIClient
{
    public interface IAccessTokenProvider
    {
        string? GetAccessToken();
    }
}
