using InventoryTracker.Application.Features.Countries.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Countries.Commands.CreateCountry
{
    public class CreateCountryCommand: IRequest<CountryDTO>
    {
        public string Name { get; set; } = default!;
        public string Code { get; set; } = default!;
    }
}
