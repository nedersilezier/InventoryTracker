using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Clients.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using InventoryTracker.Domain.Entities;
using InventoryTracker.Application.Common.DTOs;

namespace InventoryTracker.Application.Features.Clients.Commands.CreateClient
{
    public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, ClientDTO>
    {
        private readonly IAppDbContext _context;
        public CreateClientCommandHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<ClientDTO> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            var clientCodeExists = await _context.Clients.AnyAsync(c => c.ClientCode == request.ClientCode, cancellationToken);
            if (clientCodeExists)
                throw new InvalidOperationException($"Client with code {request.ClientCode} already exists.");

            var country = await _context.Countries.FirstOrDefaultAsync(c => c.CountryId == request.Address.CountryId, cancellationToken);
            if(country == null)
                throw new InvalidOperationException($"Country with id {request.Address.CountryId} does not exist.");

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

            var client = new Client
            {
                ClientId = Guid.NewGuid(),
                Name = request.Name,
                ClientCode = request.ClientCode,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                AddressId = address.AddressId,
                Address = address,
                IsActive = true,
            };
            _context.Addresses.Add(address);
            _context.Clients.Add(client);
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
                    CountryId = client.Address.CountryId
                }
            };
        }
    }
}