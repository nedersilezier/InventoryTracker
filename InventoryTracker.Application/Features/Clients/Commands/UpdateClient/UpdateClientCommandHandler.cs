using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Clients.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using InventoryTracker.Application.Common.DTOs;

namespace InventoryTracker.Application.Features.Clients.Commands.UpdateClient
{
    public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, ClientDTO?>
    {
        private readonly IAppDbContext _context;
        public UpdateClientCommandHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<ClientDTO?> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
        {
            var client = await _context.Clients.Include(c => c.Address).ThenInclude(a => a.Country).FirstOrDefaultAsync(c => c.ClientId == request.ClientId, cancellationToken);
            if (client == null)
                throw new InvalidOperationException($"Client with id {request.ClientId} not found.");

            var country = await _context.Countries.FirstOrDefaultAsync(c => c.CountryId == request.Address.CountryId, cancellationToken);
            if (country == null)
                throw new InvalidOperationException($"Country with id {request.Address.CountryId} does not exist.");

            var addressExists = await _context.Addresses.AnyAsync(a => a.AddressId == client.AddressId, cancellationToken);
            if (!addressExists)
                throw new InvalidOperationException($"Address with id {client.AddressId} does not exist.");

            client.Name = request.Name;
            client.ClientCode = request.ClientCode;
            client.Email = request.Email;
            client.PhoneNumber = request.PhoneNumber;
            client.Address.Street = request.Address.Street;
            client.Address.HouseNumber = request.Address.HouseNumber;
            client.Address.ApartmentNumber = request.Address.ApartmentNumber;
            client.Address.PostalCode = request.Address.PostalCode;
            client.Address.City = request.Address.City;
            client.Address.CountryId = request.Address.CountryId;

            await _context.SaveChangesAsync(cancellationToken);
            return new ClientDTO
            {
                ClientId = client.ClientId,
                Name = client.Name,
                ClientCode = client.ClientCode,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber,
                Address = new AddressDTO
                {
                    AddressId = client.AddressId,
                    Street = client.Address.Street,
                    City = client.Address.City,
                    HouseNumber = client.Address.HouseNumber,
                    ApartmentNumber = client.Address.ApartmentNumber,
                    PostalCode = client.Address.PostalCode,
                    CountryId = country.CountryId
                },
                IsActive = client.IsActive
            };
        }
    }
}
