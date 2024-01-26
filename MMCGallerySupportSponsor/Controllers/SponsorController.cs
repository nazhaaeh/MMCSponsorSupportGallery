using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using Infrastrecture.Repositories;
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
        public SponsorController( IUnitOfWork untofwork, IMapper mapper, IWebHostEnvironment webHostEnvironment)

        {
            _untofwork = untofwork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
      public async Task<IActionResult> GetAllSponsors()
        {
            var sponsor = await _untofwork.Sponsor.GetAllAsync();
            var sponsorDto = _mapper.Map<IEnumerable<SponsorDto>>(sponsor);
            return Ok(sponsorDto);
        }
        [HttpGet ("{id}")]
        public async Task<IActionResult> GetSponsorById(Guid id)
        {
            var spnonsor = await _untofwork.Sponsor.GetByIdAsync(id);
            if (spnonsor == null)
            {
                return NotFound();
            }
            var sponsorDto = _mapper.Map<SponsorDto>(spnonsor);
            return Ok(sponsorDto);
        }
        [HttpPost]
        public async Task<IActionResult> CreateSponsors([FromForm] SponsorCreateDto CreateSponsor)
        {
           
            if (CreateSponsor.SponsorImage !=null && CreateSponsor.SponsorImage.Length >= 10 * 1024 *1024 )
            {
                return BadRequest("L'image depasse 10Mo");

            }
            var AcceptableExtention = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var FileExtentions = Path.GetExtension(CreateSponsor.SponsorImage.FileName).ToLower();
            if (!AcceptableExtention.Contains(FileExtentions))
            {
                return BadRequest("Le format du fichier n'est pas autorisé. Veuillez choisir un fichier  jpg, jpeg, png, .gif.");

            }
            var sponsor = _mapper.Map<Sponsor>(CreateSponsor);
            if (CreateSponsor.SponsorImage != null && CreateSponsor.SponsorImage.FileName != null)
            {
                var UplaodFolder = Path.Combine(_webHostEnvironment.WebRootPath, "UplaodSponsorImage");
                var UniqueFileName = Guid.NewGuid().ToString() + "_" + CreateSponsor.SponsorImage.FileName;
                var filepath = Path.Combine(UplaodFolder, UniqueFileName);
                using (var fileStream = new FileStream(filepath, FileMode.Create))
                {
                    await CreateSponsor.SponsorImage.CopyToAsync(fileStream);
                }
                sponsor.ImagesponsorPath = filepath;
            }
            _untofwork.Sponsor.AddAsync(sponsor);
            await _untofwork.SaveAsync();
            var spnsorDto = _mapper.Map<SponsorDto>(sponsor);

            return CreatedAtAction(nameof(GetSponsorById), new {id = spnsorDto.Idsponsor }, spnsorDto);
        }
        [HttpDelete("{id}") ]
        public async Task<IActionResult> DeleteSponsor( Guid id)
        {
            var exestingSponsor = await _untofwork.Sponsor.GetByIdAsync(id);
            if (exestingSponsor == null)
            {
                return NotFound();

            }
            if (!string.IsNullOrEmpty(exestingSponsor.ImagesponsorPath))
                { 
            
                System.IO.File.Delete(exestingSponsor.ImagesponsorPath);
            }
            _untofwork.Sponsor.Remove(exestingSponsor);
            await _untofwork.SaveAsync();
            return Ok("it's deleted");
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupport(Guid id, [FromForm] SponsorUpdateDto updateDto)
        {

            var existingsponsor = await _untofwork.Sponsor.GetByIdAsync(id);

            if (existingsponsor == null)
            {
                return NotFound();
            }

            // Supprimer l'ancien fichier PDF s'il existe
            if (!string.IsNullOrEmpty(existingsponsor.ImagesponsorPath))
            {
                System.IO.File.Delete(existingsponsor.ImagesponsorPath);
            }

            // Enregistrer le nouveau fichier PDF s'il est fourni
            if (updateDto.SponsorImage != null)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "UplaodSponsorImage");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + updateDto.SponsorImage.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await updateDto.SponsorImage.CopyToAsync(fileStream);
                }

                existingsponsor.ImagesponsorPath = filePath;
            }

            _mapper.Map(updateDto, existingsponsor);
            _untofwork.Sponsor.Update(existingsponsor);
            await _untofwork.SaveAsync();

            return NoContent();
        }
    }
}
