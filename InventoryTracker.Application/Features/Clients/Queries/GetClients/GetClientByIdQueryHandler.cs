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
    public class GetClientByIdQueryHandler : IRequestHandler<GetClientByIdQuery, ClientDTO>
    {
        private readonly IAppDbContext _context;
        public GetClientByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<ClientDTO> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
        {
            var client = await _context.Clients
                .AsNoTracking()
                .Where(c => c.ClientId == request.ClientId)
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
                throw new InvalidOperationException($"Client with id {request.ClientId} not found.");
            return client;
        }
    }
}
