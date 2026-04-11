using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Warehouses.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Common.Exceptions;
using InventoryTracker.Domain.Entities;

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
            var warehouse = await _context.Warehouses.Include(w => w.Address).FirstOrDefaultAsync(w => w.WarehouseId == request.WarehouseId, cancellationToken);
            if (warehouse == null)
                throw new RecordNotFoundException(nameof(Warehouse), request.WarehouseId);

            var warehouseCodeExists = await _context.Warehouses.AnyAsync(w => w.Code == request.Code && w.WarehouseId != request.WarehouseId, cancellationToken);
            if (warehouseCodeExists)
                throw new BusinessException($"Another warehouse with code {request.Code} already exists.");

            var country = await _context.Countries.FirstOrDefaultAsync(c => c.CountryId == request.Address.CountryId, cancellationToken);
            if (country == null)
                throw new RecordNotFoundException(nameof(Country), request.Address.CountryId);
            var address = warehouse.Address;
            if (address == null)
                throw new RecordNotFoundException(nameof(Address), warehouse.Address.AddressId);

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
                    AddressId = warehouse.Address.AddressId,
                    Street = warehouse.Address.Street,
                    City = warehouse.Address.City,
                    HouseNumber = warehouse.Address.HouseNumber,
                    ApartmentNumber = warehouse.Address.ApartmentNumber,
                    PostalCode = warehouse.Address.PostalCode,
                    CountryName = country.Name,
                    CountryId = country.CountryId
                }
            };
        }
    }
}
