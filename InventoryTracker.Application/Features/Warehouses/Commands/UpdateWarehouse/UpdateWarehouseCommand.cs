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
        public UpdateWarehouseAddressDTO Address { get; set; } = default!;
        public class UpdateWarehouseAddressDTO
        {
            public string Street { get; set; } = default!;
            public string HouseNumber { get; set; } = default!;
            public string? ApartmentNumber { get; set; }
            public string PostalCode { get; set; } = default!;
            public string City { get; set; } = default!;
            public Guid CountryId { get; set; }
        }
    }
}
