using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTO;
using MediatR;
using Application.SupportsCQRS.Queries;
using Application.SupportsCQRS.Commandes;

namespace MMCGallerySupportSponsor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMediator _mediatR; 
        public SupportController(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment, IMediator mediatR)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _mediatR = mediatR;

        }
        [HttpGet]
        public async Task<IActionResult> GetAllSupports()
        {
            var Query = new GetSupportQueryRequest();
            var  result = await _mediatR.Send(Query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupportById(Guid id)
        {
            var Query = new GetBydSponsorQueryRequest();
            Query.id = id;
            var result = await _mediatR.Send(Query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSupport([FromForm] SupportCreateDto createDto)
        {
            var Commandes = new CreateSupportCommandeRequest(createDto);
         
            var result = await _mediatR.Send(Commandes);
            return Ok(result);

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> SupportDelete(Guid id)
        {
            var command = new DeleteSupportCommandeRequest
            {
                Id = id
            };

            await _mediatR.Send(command);

            return Ok("It's deleted");
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupport(Guid id, [FromForm] SupportUpdateDto updateDto)
        {
            var command = new UpdateSupportCommandeRequest
            {
                SupportId = id,
                SupportUpdateRequest = updateDto
            };

            var updatedSupportDto = await _mediatR.Send(command);

            if (updatedSupportDto == null)
            {
                return NotFound();
            }

            return NoContent();
        }


    }
}
