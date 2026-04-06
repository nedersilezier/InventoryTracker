using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Warehouses.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using InventoryTracker.Application.Common.DTOs;

namespace InventoryTracker.Application.Features.Warehouses.Commands.UpdateWarehouse
{
    public class UpdateWarehouseCommandHandler : IRequestHandler<UpdateWarehouseCommand, WarehouseDTO?>
    {
        private readonly IAppDbContext _context;
        public UpdateWarehouseCommandHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<WarehouseDTO?> Handle(UpdateWarehouseCommand request, CancellationToken cancellationToken)
        {
            var warehouse = await _context.Warehouses.FirstOrDefaultAsync(w => w.WarehouseId == request.WarehouseId, cancellationToken);
            if (warehouse == null)
                throw new InvalidOperationException($"Warehouse with id {request.WarehouseId} not found.");

            var warehouseCodeExists = await _context.Warehouses.AnyAsync(w => w.Code == request.Code && w.WarehouseId != request.WarehouseId, cancellationToken);
            if (warehouseCodeExists)
                throw new InvalidOperationException($"Another warehouse with code {request.Code} already exists.");

            var countryExists = await _context.Countries.AnyAsync(c => c.CountryId == request.Address.CountryId, cancellationToken);
            if (!countryExists)
                throw new InvalidOperationException($"Country with id {request.Address.CountryId} not found.");
            var addressExists = await _context.Addresses.AnyAsync(a => a.AddressId == request.Address.AddressId, cancellationToken);
            if (!addressExists)
                throw new InvalidOperationException($"Address with id {request.Address.AddressId} not found.");

            warehouse.Name = request.Name;
            warehouse.Code = request.Code;
            warehouse.Address.Street = request.Address.Street;
            warehouse.Address.HouseNumber = request.Address.HouseNumber;
            warehouse.Address.ApartmentNumber = request.Address.ApartmentNumber;
            warehouse.Address.PostalCode = request.Address.PostalCode;
            warehouse.Address.City = request.Address.City;
            warehouse.Address.CountryId = request.Address.CountryId;

            await _context.SaveChangesAsync(cancellationToken);
            return new WarehouseDTO
            {
                WarehouseId = warehouse.WarehouseId,
                Name = warehouse.Name,
                Code = warehouse.Code,
                Address = new AddressDTO
                {
                    Street = warehouse.Address.Street,
                    City = warehouse.Address.City,
                    HouseNumber = warehouse.Address.HouseNumber,
                    ApartmentNumber = warehouse.Address.ApartmentNumber,
                    PostalCode = warehouse.Address.PostalCode,
                    CountryName = warehouse.Address.Country.Name,
                    CountryId = warehouse.Address.CountryId
                }
            };
        }
    }
}
