using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.GalleryCQRS.Commandes
{
    public class UpdateGalleryCommandeHandler : IRequestHandler<UpdateGalleryCommandeRequest, GalleryUpdateDto>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UpdateGalleryCommandeHandler(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<GalleryUpdateDto> Handle(UpdateGalleryCommandeRequest request, CancellationToken cancellationToken)
        {
            var existingGallery = await _unitOfWork.Gallery.GetByIdAsync(request.GalleryId);

            if (existingGallery  == null)
            {
                return null;
            }
            var ImagName = $"Img_Gallery_{existingGallery .GalleryId}";
            var ImagExten = Path.GetExtension(request.GalleryUpdateRequest.GalleryFile.FileName);
            // Supprimer l'ancien fichier image s'il existe
            if (!string.IsNullOrEmpty(existingGallery .imageGalleryPath))
            {
                System.IO.File.Delete(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot/uploadsGallery/", $"{ImagName}{ImagExten}"));
            }
            string hosturl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";

            // Enregistrer le nouveau fichier image s'il est fourni
            if (request.GalleryUpdateRequest.GalleryFile != null)
            {
                // Enregistrez l'image dans le dossier wwwroot/UplaodSponsorImage

                var localeFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot/uploadsGallery/", $"{ImagName}{ImagExten}");

                using (var fileStream = new FileStream(localeFilePath, FileMode.Create))
                {
                    await request.GalleryUpdateRequest.GalleryFile.CopyToAsync(fileStream);
                }

                existingGallery.imageGalleryPath = $"{hosturl}/uploads/{ImagName}{ImagExten}";


            }

            // Mapper les propriétés du DTO à l'entité existante
            _mapper.Map(request.GalleryUpdateRequest, existingGallery );

            // Mettre à jour l'entité dans la base de données
            _unitOfWork.Gallery.Update(existingGallery );
            await _unitOfWork.SaveAsync();

            // Mapper l'entité mise à jour vers le DTO de réponse
            var updatedGalleryDto = _mapper.Map<GalleryUpdateDto>(existingGallery );

            return updatedGalleryDto;
        }
    }
}
