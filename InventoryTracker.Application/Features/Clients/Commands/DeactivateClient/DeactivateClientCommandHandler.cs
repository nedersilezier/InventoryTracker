using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Clients.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Clients.Commands.DeactivateClient
{
    public class DeactivateClientCommandHandler : IRequestHandler<DeactivateClientCommand, ClientDTO?>
    {
        private readonly IAppDbContext _context;
        public DeactivateClientCommandHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<ClientDTO?> Handle(DeactivateClientCommand request, CancellationToken cancellationToken)
        {
            var client = await _context.Clients
                .Include(c => c.Address)
                .ThenInclude(a => a.Country)
                .FirstOrDefaultAsync(c => c.ClientId == request.ClientId, cancellationToken);
            if (client == null)
                return null;
            client.IsActive = false;
            await _context.SaveChangesAsync(cancellationToken);
            return new ClientDTO
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
                    HouseNumber = client.Address.HouseNumber,
                    ApartmentNumber = client.Address.ApartmentNumber,
                    PostalCode = client.Address.PostalCode,
                    City = client.Address.City,
                    CountryId = client.Address.CountryId,
                    CountryName = client.Address.Country.Name
                }
            };
        }
    }
}
