using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Clients.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Clients.Queries.GetClients
{
    public class GetClientsQueryHandler : IRequestHandler<GetClientsQuery, List<ClientDTO>>
    {
        private readonly IAppDbContext _context;
        public GetClientsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<ClientDTO>> Handle(GetClientsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Clients
                .AsNoTracking()
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
                        CountryId = c.Address.CountryId,
                        CountryName = c.Address.Country.Name
                    }
                }).ToListAsync();
        }
    }
}
