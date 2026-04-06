using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Warehouses.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Warehouses.Queries.GetWarehouses
{
    public class GetWarehouseByIdQueryHandler : IRequestHandler<GetWarehouseByIdQuery, WarehouseDTO?>
    {
        private readonly IAppDbContext _context;
        public GetWarehouseByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<WarehouseDTO?> Handle(GetWarehouseByIdQuery request, CancellationToken cancellationToken)
        {
            var warehouse = await _context.Warehouses
                .AsNoTracking()
                .Where(w => w.WarehouseId == request.WarehouseId)
                .Select(w => new WarehouseDTO
                {
                    WarehouseId = w.WarehouseId,
                    Name = w.Name,
                    Code = w.Code,
                    Address = new AddressDTO
                    {
                        Street = w.Address.Street,
                        City = w.Address.City,
                        HouseNumber = w.Address.HouseNumber,
                        ApartmentNumber = w.Address.ApartmentNumber,
                        PostalCode = w.Address.PostalCode,
                        CountryName = w.Address.Country.Name,
                        CountryId = w.Address.Country.CountryId
                    }
                }).FirstOrDefaultAsync(cancellationToken);
            if (warehouse == null)
            {
                throw new InvalidOperationException($"Warehouse with id {request.WarehouseId} not found.");
            }
            return warehouse;
        }
    }
}
