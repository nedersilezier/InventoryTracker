using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Warehouses.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using InventoryTracker.Domain.Entities;
using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Common.Exceptions;

namespace InventoryTracker.Application.Features.Warehouses.Commands.CreateWarehouse
{
    public class CreateWarehouseCommandHandler : IRequestHandler<CreateWarehouseCommand, WarehouseDTO>
    {
        private readonly IAppDbContext _context;
        public CreateWarehouseCommandHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<WarehouseDTO> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
        {
            var warehouseCodeExists = await _context.Warehouses.AnyAsync(w => w.Code == request.Code, cancellationToken);
            if (warehouseCodeExists)
                throw new BusinessException($"Warehouse with code '{request.Code}' already exists.");
            var country = await _context.Countries.FirstOrDefaultAsync(c => c.CountryId == request.Address.CountryId, cancellationToken);
            if (country == null)
                throw new RecordNotFoundException(nameof(Country), request.Address.CountryId);

            var address = new Address
            {
                AddressId = Guid.NewGuid(),
                Street = request.Address.Street,
                City = request.Address.City,
                HouseNumber = request.Address.HouseNumber,
                ApartmentNumber = request.Address.ApartmentNumber,
                PostalCode = request.Address.PostalCode,
                CountryId = request.Address.CountryId,
                Country = country
            };
            var warehouse = new Warehouse
            {
                WarehouseId = Guid.NewGuid(),
                Name = request.Name,
                Code = request.Code,
                AddressId = address.AddressId,
                Address = address,
                IsActive = true
            };
            _context.Addresses.Add(address);
            _context.Warehouses.Add(warehouse);
            await _context.SaveChangesAsync(cancellationToken);
            return new WarehouseDTO
            {
                WarehouseId = warehouse.WarehouseId,
                Name = warehouse.Name,
                Code = warehouse.Code,
                Address = new AddressDTO
                {
                    AddressId = address.AddressId,
                    Street = address.Street,
                    City = address.City,
                    HouseNumber = address.HouseNumber,
                    ApartmentNumber = address.ApartmentNumber,
                    PostalCode = address.PostalCode,
                    CountryId = address.CountryId
                }
            };
        }
    }
}
