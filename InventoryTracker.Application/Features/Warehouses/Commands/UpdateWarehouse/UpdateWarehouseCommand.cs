using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Features.Warehouses.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Warehouses.Commands.UpdateWarehouse
{
    public class UpdateWarehouseCommand: IRequest<WarehouseDTO?>
    {
        public Guid WarehouseId { get; set; }
        public string Name { get; set; } = default!;
        public string Code { get; set; } = default!;
        public AddressDTO Address { get; set; } = default!;
    }
}
