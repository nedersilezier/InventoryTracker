using InventoryTracker.Application.Features.Countries.Commands.CreateCountry;
using InventoryTracker.Application.Features.Countries.Commands.DeleteCountry;
using InventoryTracker.Application.Features.Countries.Commands.UpdateCountry;
using InventoryTracker.Application.Features.Countries.Queries.GetCountries;
using InventoryTracker.Contracts.Requests.Countries;
using InventoryTracker.Contracts.Responses.Common;
using InventoryTracker.Contracts.Responses.Countries;
using InventoryTracker.Contracts.Responses.Users;
using InventoryTracker.Domain.Entities;
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
        public async Task<IActionResult> GetAllCountries([FromQuery] GetCountriesRequest request, CancellationToken cancellationToken)
        {
            var query = new GetCountriesQuery
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize ?? 10,
                SearchTerm = request.SearchTerm
            };
            var countriesPaged = await _mediator.Send(query, cancellationToken);
            var response = new PagedResponse<CountryResponseDTO>
            {
                Items = countriesPaged.Items.Select(country => new CountryResponseDTO
                {
                    CountryId = country.CountryId,
                    Name = country.Name,
                    Code = country.Code,
                    CreatedBy = country.CreatedBy ?? string.Empty,
                    CreatedAt = country.CreatedAt,
                    UpdatedBy = country.UpdatedBy,
                    UpdatedAt = country.UpdatedAt
                }).ToList(),

                TotalPages = countriesPaged.TotalPages,
                PageNumber = countriesPaged.PageNumber,
                PageSize = countriesPaged.PageSize,
                TotalCount = countriesPaged.TotalCount
            };
            return Ok(response);
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

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCountry(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteCountryCommand
            {
                CountryId = id
            };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}
