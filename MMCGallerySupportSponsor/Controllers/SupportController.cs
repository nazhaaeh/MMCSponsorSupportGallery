using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Infrastrecture.Repositories;
using Application.Interfaces;
using Application.DTO;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MMCGallerySupportSponsor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SupportController(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;

        }
        [HttpGet]
        public async Task<IActionResult> GetAllSupports()
        {
            var supports = await _unitOfWork.Support.GetAllAsync();
            var supportDtos = _mapper.Map<IEnumerable<SupportDto>>(supports);

            return Ok(supportDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupportById(Guid id)
        {
            var support = await _unitOfWork.Support.GetByIdAsync(id);

            if (support == null)
            {
                return NotFound();
            }

            var supportDto = _mapper.Map<SupportDto>(support);
            return Ok(supportDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSupport([FromForm] SupportCreateDto createDto)
        {

            if (createDto.File != null && createDto.File.Length >= 10 * 1024 * 1024)
            {
                return BadRequest("Ce fichier depasse 10 Mo ");

            }
            var AcceptedExtensions = new[] { ".pdf", ".docx" };
            var FileExtentions = Path.GetExtension(createDto.File.FileName).ToLower();
            if (!AcceptedExtensions.Contains(FileExtentions))
            {
                return BadRequest("Le format du fichier n'est pas autorisé. Veuillez choisir un fichier PDF, DOCX.");

            }

            
            var support = _mapper.Map<Support>(createDto);

            if (createDto.File != null && createDto.File.FileName != null)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + createDto.File.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await createDto.File.CopyToAsync(fileStream);
                }

                support.FilePath = filePath;
            }

             _unitOfWork.Support.AddAsync(support);
              await _unitOfWork.SaveAsync();


            var supportDto = _mapper.Map<SupportDto>(support);
           

            return CreatedAtAction(nameof(GetSupportById), new { id = supportDto.Idsuppport }, supportDto);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SupportDelate(Guid id)
        {
            var exestingSupport = await _unitOfWork.Support.GetByIdAsync(id);
            if (exestingSupport == null)
            {
                return NotFound();

            }
            if (!string.IsNullOrEmpty(exestingSupport.FilePath))
            {
                System.IO.File.Delete(exestingSupport.FilePath);
            }
            _unitOfWork.Support.Remove(exestingSupport);
            await _unitOfWork.SaveAsync();
            return Ok("it's deleted");
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupport(Guid id, [FromForm] SupportUpdateDto updateDto)
        {
            
            var existingSupport = await _unitOfWork.Support.GetByIdAsync(id);

            if (existingSupport == null)
            {
                return NotFound();
            }

            // Supprimer l'ancien fichier PDF s'il existe
            if (!string.IsNullOrEmpty(existingSupport.FilePath))
            {
                System.IO.File.Delete(existingSupport.FilePath);
            }

            // Enregistrer le nouveau fichier PDF s'il est fourni
            if (updateDto.File != null)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + updateDto.File.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await updateDto.File.CopyToAsync(fileStream);
                }

                existingSupport.FilePath = filePath; 
            }

        
            _mapper.Map(updateDto, existingSupport);

            _unitOfWork.Support.Update(existingSupport);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }


    }
}
