using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Countries.Commands.DeleteCountry
{
    public class DeleteCountryCommand: IRequest<Unit>
    {
        public Guid CountryId { get; set; }
    }
}
