using InventoryTracker.Contracts.Requests.Countries;
using InventoryTracker.Application.Features.Countries.Commands.CreateCountry;
using InventoryTracker.Application.Features.Countries.Commands.UpdateCountry;
using InventoryTracker.Application.Features.Countries.Queries.GetCountries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/countries")]
    [Authorize]
    public class CountriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CountriesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCountries(CancellationToken cancellationToken)
        {
            var countries = await _mediator.Send(new GetCountriesQuery(), cancellationToken);
            return Ok(countries);
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCountryById(Guid id, CancellationToken cancellationToken)
        {
            var country = await _mediator.Send(new GetCountryByIdQuery(id), cancellationToken);
            if (country == null)
                return NotFound();
            return Ok(country);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCountry(CreateCountryCommand command, CancellationToken cancellationToken)
        {
            var country = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetCountryById), new { id = country.CountryId }, country);
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateCountry(Guid id, UpdateCountryRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateCountryCommand
            {
                CountryId = id,
                Name = request.Name,
                Code = request.Code
            };
            var country = await _mediator.Send(command, cancellationToken);
            if (country == null)
                return NotFound();
            return Ok(country);
        }
    }
}
