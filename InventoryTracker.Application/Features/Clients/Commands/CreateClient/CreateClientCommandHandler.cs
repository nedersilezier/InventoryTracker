using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Clients.DTOs;
using MediatR;
using InventoryTracker.Domain.Entities;
using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Common.Exceptions;

namespace InventoryTracker.Application.Features.Clients.Commands.CreateClient
{
    public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, ClientDTO>
    {
        private readonly IAppDbContext _context;
        private readonly IAddressesRepository _addressesRepository;
        private readonly IClientsRepository _clientsRepository;
        private readonly ICountriesRepository _countriesRepository;
        public CreateClientCommandHandler(IAppDbContext context, IAddressesRepository addressesRepository, IClientsRepository clientsRepository, ICountriesRepository countriesRepository)
        {
            _context = context;
            _addressesRepository = addressesRepository;
            _clientsRepository = clientsRepository;
            _countriesRepository = countriesRepository;
        }
        public async Task<ClientDTO> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            var clientCodeExists = await _clientsRepository.ClientCodeExistsAsync(request.ClientCode, cancellationToken);
            if (clientCodeExists)
                throw new BusinessException($"Client with code {request.ClientCode} already exists.");

            var countryExists = await _countriesRepository.CountryExistsAsync(request.Address.CountryId, cancellationToken);
            if (!countryExists)
                throw new RecordNotFoundException(nameof(Country), request.Address.CountryId);

            var address = new Address
            {
                AddressId = Guid.NewGuid(),
                Street = request.Address.Street,
                City = request.Address.City,
                HouseNumber = request.Address.HouseNumber,
                ApartmentNumber = request.Address.ApartmentNumber,
                PostalCode = request.Address.PostalCode,
                CountryId = request.Address.CountryId
            };

            var client = new Client
            {
                ClientId = Guid.NewGuid(),
                Name = request.Name,
                ClientCode = request.ClientCode,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                AddressId = address.AddressId,
                IsActive = true,
            };
            await _addressesRepository.AddAddress(address);
            await _clientsRepository.AddClient(client);
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
                    AddressId = address.AddressId,
                    Street = address.Street,
                    HouseNumber = address.HouseNumber,
                    ApartmentNumber = address.ApartmentNumber,
                    PostalCode = address.PostalCode,
                    City = address.City,
                    CountryId = address.CountryId
                }
            };
        }
    }
}