using InventoryTracker.Application.Features.Countries.DTOs;
using MediatR;
using InventoryTracker.Application.Common.Interfaces;

namespace InventoryTracker.Application.Features.Countries.Commands.UpdateCountry
{
    public class UpdateCountryCommandHandler : IRequestHandler<UpdateCountryCommand, CountryDTO?>
    {
        private readonly ICountriesService _countriesService;
        public UpdateCountryCommandHandler(ICountriesService countriesService)
        {
            _countriesService = countriesService;
        }
        public async Task<CountryDTO?> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
        {
            request.Name = request.Name.Trim();
            var nameSplit = request.Name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < nameSplit.Length; i++)
            {
                nameSplit[i] = char.ToUpper(nameSplit[i][0]) + nameSplit[i].Substring(1).ToLower();
            }
            request.Name = string.Join(' ', nameSplit);
            request.Code = request.Code.Trim().ToUpper();

            var parameters = new UpdateCountryParameters
            {
                CountryId = request.CountryId,
                Name = request.Name,
                Code = request.Code
            };
            return await _countriesService.UpdateCountryAsync(parameters, cancellationToken);
        }
    }
}