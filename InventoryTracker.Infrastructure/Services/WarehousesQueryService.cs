using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Common.Exceptions;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Clients.DTOs;
using InventoryTracker.Application.Features.Warehouses.DTOs;
using InventoryTracker.Application.Features.Warehouses.Queries.GetWarehouses;
using InventoryTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryTracker.Infrastructure.Services
{
    public class WarehousesQueryService: IWarehousesQueryService
    {
        private readonly AppDbContext _context;
        public WarehousesQueryService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<WarehouseDTO?> GetWarehouseByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var warehouse = await _context.Warehouses
                .Include(w => w.Address)
                .ThenInclude(a => a.Country)
                .AsNoTracking()
                .Where(w => w.WarehouseId == id)
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
                        CountryId = w.Address.Country.CountryId
                    }
                }).FirstOrDefaultAsync(cancellationToken);
            return warehouse;
        }
        public async Task<PagedResult<WarehouseDTO>> GetAllWarehousesAsync(GetWarehousesParameters parameters, CancellationToken cancellationToken)
        {
            var query = _context.Warehouses.AsQueryable();
            if (!string.IsNullOrEmpty(parameters.SearchTerm))
            {
                query = query.Where(c => c.Name.Contains(parameters.SearchTerm)
                                    || c.Code.Contains(parameters.SearchTerm)
                                    || c.Address.City.Contains(parameters.SearchTerm)
                                    || c.Address.Country.Name.Contains(parameters.SearchTerm));
            }
            var totalCount = await query.CountAsync(cancellationToken);
            var totalPages = totalCount == 0 ? 1 : (int)Math.Ceiling((double)totalCount / parameters.PageSize);
            var pageNumber = parameters.PageNumber;
            if (pageNumber > totalPages)
                pageNumber = totalPages;

            query = query.Include(c => c.Address).ThenInclude(a => a.Country);
            var warehouses = await query.OrderBy(c => c.Name).Skip((pageNumber - 1) * parameters.PageSize).Take(parameters.PageSize).ToListAsync(cancellationToken);
            var warehousesDTO = new List<WarehouseDTO>();
            foreach (var warehouse in warehouses)
            {
                warehousesDTO.Add(new WarehouseDTO
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
                        CountryName = warehouse.Address.Country.Name,
                        CountryId = warehouse.Address.CountryId
                    }
                });

            }

            return new PagedResult<WarehouseDTO>
            {
                Items = warehousesDTO,
                TotalPages = totalPages,
                PageNumber = pageNumber,
                PageSize = parameters.PageSize,
                TotalCount = totalCount
            };
        }
    }
}
