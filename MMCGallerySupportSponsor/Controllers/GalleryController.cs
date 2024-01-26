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
using Microsoft.IdentityModel.Tokens;

namespace MMCGallerySupportSponsor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GalleryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public GalleryController(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;

        }
        [HttpGet]
        public async Task<IActionResult> GetAllGallery()
        {
            var gallery = await _unitOfWork.Gallery.GetAllAsync();
            var galleryDto = _mapper.Map<IEnumerable<GalleryDto>>(gallery);
    
            return Ok(galleryDto);
        }
        [HttpGet ("{id}")]
        public async Task<IActionResult> GetGalleryById(Guid id )
        {
            var gallery = await _unitOfWork.Gallery.GetByIdAsync(id);
            if(gallery == null)
            {
                return NotFound();
            }
            var gallyDto = _mapper.Map<GalleryDto>(gallery);
            return Ok(gallery);

        }

        [HttpPost]
        public async Task<IActionResult> CreateGallery([FromForm] GalleryCreateDto createDto)
        {
            if (createDto.GalleryFile != null && createDto.GalleryFile.Length >= 10 * 1024 * 1024)
            {
                return BadRequest("Ce fichier depasse 10 Mo ");

            }
            var AcceptedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var FileExtentions = Path.GetExtension(createDto.GalleryFile.FileName).ToLower();
            if (!AcceptedExtensions.Contains(FileExtentions))
            {
                return BadRequest("Le format du fichier n'est pas autorisé. Veuillez choisir un fichier .jpg, .jpeg, .png, .gif ");

            }
            var gallery = _mapper.Map<Gallery>(createDto);
           


            if (createDto.GalleryFile != null && createDto.GalleryFile.FileName != null)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploadsGallery");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + createDto.GalleryFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await createDto.GalleryFile.CopyToAsync(fileStream);
                }

                gallery.imageGalleryPath = filePath;
            }
            _unitOfWork.Gallery.AddAsync(gallery);
            await _unitOfWork.SaveAsync();


             var galleryDto = _mapper.Map<Gallery>(gallery);


            return CreatedAtAction(nameof(GetGalleryById), new { id = galleryDto.GalleryId }, galleryDto);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGallery( Guid id)
        {
            var exestingGallery = await _unitOfWork.Gallery.GetByIdAsync(id);
            if (exestingGallery == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrEmpty(exestingGallery.imageGalleryPath))
            {
                System.IO.File.Delete(exestingGallery.imageGalleryPath);
                
            }
            _unitOfWork.Gallery.Remove(exestingGallery);
            await _unitOfWork.SaveAsync();
            return Ok("Image deleted!");

        }
        [HttpPut ("{id}")]
        public async Task<IActionResult> UpdateGallery(Guid id , [FromForm] GalleryUpdateDto updateDto)
        {
            var existingSupport = await _unitOfWork.Gallery.GetByIdAsync(id);

            if (existingSupport == null)
            {
                return NotFound();
            }

            // Supprimer l'ancien fichier PDF s'il existe
            if (!string.IsNullOrEmpty(existingSupport.imageGalleryPath))
            {
                System.IO.File.Delete(existingSupport.imageGalleryPath);
            }

            // Enregistrer le nouveau fichier PDF s'il est fourni
            if (updateDto.GalleryFile != null)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploadsGallery");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + updateDto.GalleryFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await updateDto.GalleryFile.CopyToAsync(fileStream);
                }

                existingSupport.imageGalleryPath = filePath;
            }


            _mapper.Map(updateDto, existingSupport);

            _unitOfWork.Gallery.Update(existingSupport);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    } 
}
