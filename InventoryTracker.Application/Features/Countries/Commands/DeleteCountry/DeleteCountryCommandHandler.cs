using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Countries.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Countries.Commands.DeleteCountry
{
    public class DeleteCountryCommandHandler : IRequestHandler<DeleteCountryCommand, Unit>
    {
        private readonly ICountriesService _countriesService;
        public DeleteCountryCommandHandler(ICountriesService countriesService)
        {
            _countriesService = countriesService;
        }
        public async Task<Unit> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
        {
            await _countriesService.DeleteCountryAsync(request.CountryId, cancellationToken);
            return Unit.Value;
        }
    }
}