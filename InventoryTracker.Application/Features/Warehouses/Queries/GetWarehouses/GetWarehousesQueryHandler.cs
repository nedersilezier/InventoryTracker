using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Warehouses.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using InventoryTracker.Application.Common.DTOs;

namespace InventoryTracker.Application.Features.Warehouses.Queries.GetWarehouses
{
    public class GetWarehousesQueryHandler : IRequestHandler<GetWarehousesQuery, List<WarehouseDTO>>
    {
        private readonly IAppDbContext _context;
        public GetWarehousesQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<WarehouseDTO>> Handle(GetWarehousesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Warehouses
                .AsNoTracking()
                .Select(w => new WarehouseDTO
                {
                    WarehouseId = w.WarehouseId,
                    Name = w.Name,
                    Code = w.Code,
                    Address = new AddressDTO
                    {
                        AddressId = w.Address.AddressId,
                        Street = w.Address.Street,
                        City = w.Address.City,
                        HouseNumber = w.Address.HouseNumber,
                        ApartmentNumber = w.Address.ApartmentNumber,
                        PostalCode = w.Address.PostalCode,
                        CountryName = w.Address.Country.Name,
                        CountryId = w.Address.CountryId
                    }
                }).ToListAsync(cancellationToken);
        }
    }
}
