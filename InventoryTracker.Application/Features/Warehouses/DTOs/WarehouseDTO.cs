using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Warehouses.DTOs
{
    public class WarehouseDTO
    {
        public Guid WarehouseId { get; set; }

        public string Name { get; set; } = default!;
        public string Code { get; set; } = default!;
        public AddressDTO Address { get; set; } = default!;
        public int StocksCount { get; set; }
    }
}
