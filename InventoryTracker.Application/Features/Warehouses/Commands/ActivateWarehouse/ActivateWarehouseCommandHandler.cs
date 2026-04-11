using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Common.Exceptions;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Warehouses.DTOs;
using InventoryTracker.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Warehouses.Commands.ActivateWarehouse
{
    public class ActivateWarehouseCommandHandler : IRequestHandler<ActivateWarehouseCommand, WarehouseDTO?>
    {
        private readonly IAppDbContext _context;
        public ActivateWarehouseCommandHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<WarehouseDTO?> Handle(ActivateWarehouseCommand request, CancellationToken cancellationToken)
        {
            var warehouse = await _context.Warehouses
                .Include(w => w.Address)
                .ThenInclude(a => a.Country)
                .FirstOrDefaultAsync(w => w.WarehouseId == request.WarehouseId, cancellationToken);

            if (warehouse == null)
                throw new RecordNotFoundException(nameof(Warehouse), request.WarehouseId);

            warehouse.IsActive = true;
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
