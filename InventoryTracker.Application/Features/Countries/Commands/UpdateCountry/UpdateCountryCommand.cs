using InventoryTracker.Application.Features.Countries.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Countries.Commands.UpdateCountry
{
    public class UpdateCountryCommand: IRequest<CountryDTO?>
    {
        public Guid CountryId { get; private set; }
        public string Name { get; set; } = default!;
        public string Code { get; set; } = default!;
    }
}
