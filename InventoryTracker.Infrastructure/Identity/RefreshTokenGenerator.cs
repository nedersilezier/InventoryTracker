using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace InventoryTracker.Infrastructure.Identity
{
    public class RefreshTokenGenerator
    {
        public string Generate()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
