using InventoryTracker.Contracts.Requests.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Contracts.Requests.Warehouses
{
    public class CreateWarehouseRequest
    {
        public string Name { get; set; } = default!;
        public string Code { get; set; } = default!;
        public CreateAddressRequest Address { get; set; } = default!;
    }
}
