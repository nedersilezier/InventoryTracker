using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Features.Warehouses.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Warehouses.Commands.CreateWarehouse
{
    public class CreateWarehouseCommand: IRequest<WarehouseDTO>
    {
        public string Name { get; set; } = default!;
        public string Code { get; set; } = default!;
        public AddressDTO Address { get; set; } = default!;
    }
}
