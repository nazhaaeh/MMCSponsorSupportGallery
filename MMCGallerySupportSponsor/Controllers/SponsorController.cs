using Application.DTO;
using Application.Interfaces;
using Application.SponsorCQRS.Commandes;
using Application.SponsorCQRS.Queries;
using Application.SupportsCQRS.Commandes;
using Application.SupportsCQRS.Queries;
using AutoMapper;
using Domain.Models;
using Infrastrecture.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MMCGallerySupportSponsor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SponsorController : ControllerBase
    {
        private readonly IUnitOfWork _untofwork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMediator _mediatR;
        public SponsorController( IUnitOfWork untofwork, IMapper mapper, IWebHostEnvironment webHostEnvironment, IMediator mediatR)

        {
            _untofwork = untofwork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _mediatR = mediatR;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllSPonsor()
        {
            var Query = new GetSponsorQueryRequest();
            var result = await _mediatR.Send(Query);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSponsorById(Guid id)
        {
            var Query = new GetBydSponsorQueryRequest();
            Query.id = id;
            var result = await _mediatR.Send(Query);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateSponsor([FromForm] SponsorCreateDto createDto)
        {
            var Commandes = new CreateSponsorQueryRequest(createDto);
            var result = await _mediatR.Send(Commandes);
            return Ok(result);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> SponsorDelete(Guid id)
        {
            var command = new DeleteSponsorCommandeRequest { Id = id };
            await _mediatR.Send(command);
            return Ok("It's deleted");
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSponsor(Guid id, [FromForm] SponsorUpdateDto updateDto)
        {
            var command = new UpdateSponsorQueryRequest
            {
                Id= id,
                SponsorUpdateRequest = updateDto
            };

            var updatedSponsorDto = await _mediatR.Send(command);

            if (updatedSponsorDto == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
