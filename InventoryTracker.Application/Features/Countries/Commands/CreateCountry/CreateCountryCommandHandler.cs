using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Countries.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Countries.Commands.CreateCountry
{
    public class CreateCountryCommandHandler: IRequestHandler<CreateCountryCommand, CountryDTO>
    {
        private readonly ICountriesService _countriesService;
        public CreateCountryCommandHandler(ICountriesService countriesService)
        {
            _countriesService = countriesService;
        }
        public async Task<CountryDTO> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
        {
            request.Name = request.Name.Trim();
            var nameSplit = request.Name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < nameSplit.Length; i++)
            {
                nameSplit[i] = char.ToUpper(nameSplit[i][0]) + nameSplit[i].Substring(1).ToLower();
            }
            request.Name = string.Join(' ', nameSplit);
            request.Code = request.Code.Trim().ToUpper();
            var parameters = new CreateCountryParameters
            {
                Name = request.Name,
                Code = request.Code
            };
            return await _countriesService.CreateCountryAsync(parameters, cancellationToken);
        }
    }
}
