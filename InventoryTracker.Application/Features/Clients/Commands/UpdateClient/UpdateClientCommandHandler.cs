using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Clients.DTOs;
using MediatR;
using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Common.Exceptions;
using InventoryTracker.Domain.Entities;

namespace InventoryTracker.Application.Features.Clients.Commands.UpdateClient
{
    public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, ClientDTO?>
    {
        private readonly IAppDbContext _context;
        private readonly IClientsRepository _clientsRepository;
        private readonly ICountriesRepository _countriesRepository;
        private readonly IAddressesRepository _addressesRepository;
        public UpdateClientCommandHandler(IAppDbContext context, IClientsRepository clientsRepository, ICountriesRepository countriesRepository, IAddressesRepository addressesRepository)
        {
            _context = context;
            _clientsRepository = clientsRepository;
            _countriesRepository = countriesRepository;
            _addressesRepository = addressesRepository;
        }
        public async Task<ClientDTO?> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
        {
            
            var client = await _clientsRepository.GetClientByIdAsync(request.ClientId, cancellationToken);
            if (client == null)
                throw new RecordNotFoundException(nameof(Client), request.ClientId);

            var clientCodeExists = await _clientsRepository.ClientCodeExistsForUpdateAsync(request.ClientCode, request.ClientId, cancellationToken);
            if(clientCodeExists)
                throw new BusinessException($"Another client with code {request.ClientCode} already exists.");

            var country = await _countriesRepository.GetCountryByIdAsync(request.Address.CountryId, cancellationToken);
            if (country == null)
                throw new RecordNotFoundException(nameof(Country), request.Address.CountryId);

            var addressExists = await _addressesRepository.AddressExistsAsync(client.AddressId, cancellationToken);
            if (!addressExists)
                throw new RecordNotFoundException(nameof(Address), client.AddressId);
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
                    CountryId = country.CountryId,
                    CountryName = country.Name
                },
                IsActive = client.IsActive
            };
        }
    }
}
