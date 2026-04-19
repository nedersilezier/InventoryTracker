using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Common.Exceptions;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Clients.Commands.CreateClient;
using InventoryTracker.Application.Features.Clients.DTOs;
using InventoryTracker.Domain.Entities;
using InventoryTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryTracker.Infrastructure.Services
{
    public class ClientsQueryService: IClientsQueryService
    {
        private readonly AppDbContext _context;
        public ClientsQueryService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<PagedResult<ClientDTO>> GetAllClientsAsync(GetClientsParameters parameters, CancellationToken cancellationToken)
        {
            var query = _context.Clients.AsQueryable();
            if (!string.IsNullOrEmpty(parameters.SearchTerm))
            {
                query = query.Where(c => c.Name.Contains(parameters.SearchTerm)
                                    || c.ClientCode.Contains(parameters.SearchTerm)
                                    || (c.Email != null && c.Email.Contains(parameters.SearchTerm))
                                    || c.Address.City.Contains(parameters.SearchTerm)
                                    || c.Address.Country.Name.Contains(parameters.SearchTerm));
            }
            var totalCount = await query.CountAsync(cancellationToken);
            var totalPages = totalCount == 0 ? 1 : (int)Math.Ceiling((double)totalCount / parameters.PageSize);
            var pageNumber = parameters.PageNumber;
            if (pageNumber > totalPages)
                pageNumber = totalPages;

            query = query.Include(c => c.Address).ThenInclude(a => a.Country);
            var clients = await query.OrderBy(c => c.Name).Skip((pageNumber - 1) * parameters.PageSize).Take(parameters.PageSize).ToListAsync(cancellationToken);
            var clientsDTO = new List<ClientDTO>();
            foreach (var client in clients)
            {
                clientsDTO.Add(new ClientDTO
                {
                    ClientId = client.ClientId,
                    Name = client.Name,
                    ClientCode = client.ClientCode,
                    Email = client.Email,
                    PhoneNumber = client.PhoneNumber,
                    IsActive = client.IsActive,
                    Address = new AddressDTO
                    {
                        AddressId = client.Address.AddressId,
                        Street = client.Address.Street,
                        City = client.Address.City,
                        HouseNumber = client.Address.HouseNumber,
                        ApartmentNumber = client.Address.ApartmentNumber,
                        PostalCode = client.Address.PostalCode,
                        CountryId = client.Address.CountryId,
                        CountryName = client.Address.Country.Name
                    }
                });

            }

            return new PagedResult<ClientDTO>
            {
                Items = clientsDTO,
                TotalPages = totalPages,
                PageNumber = pageNumber,
                PageSize = parameters.PageSize,
                TotalCount = totalCount
            };
        }
        public async Task<ClientDTO?> GetClientByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var client = await _context.Clients
                .Include(c => c.Address)
                .ThenInclude(a => a.Country)
                .AsNoTracking()
                .Where(c => c.ClientId == id)
                .Select(c => new ClientDTO
                {
                    ClientId = c.ClientId,
                    Name = c.Name,
                    ClientCode = c.ClientCode,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber,
                    IsActive = c.IsActive,
                    Address = new AddressDTO
                    {
                        AddressId = c.Address.AddressId,
                        Street = c.Address.Street,
                        City = c.Address.City,
                        HouseNumber = c.Address.HouseNumber,
                        ApartmentNumber = c.Address.ApartmentNumber,
                        PostalCode = c.Address.PostalCode,
                        CountryName = c.Address.Country.Name,
                        CountryId = c.Address.CountryId
                    }
                }).FirstOrDefaultAsync(cancellationToken);
            if (client == null)
                throw new RecordNotFoundException(nameof(Client), id);
            return client;
        }
    }
}
