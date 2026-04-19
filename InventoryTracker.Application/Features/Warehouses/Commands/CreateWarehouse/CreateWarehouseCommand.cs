using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Features.Warehouses.DTOs;
using MediatR;

namespace InventoryTracker.Application.Features.Warehouses.Commands.CreateWarehouse
{
    public class CreateWarehouseCommand: IRequest<WarehouseDTO>
    {
        public string Name { get; set; } = default!;
        public string Code { get; set; } = default!;
        public CreateWarehouseAddressDTO Address { get; set; } = default!;
        public class CreateWarehouseAddressDTO
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
