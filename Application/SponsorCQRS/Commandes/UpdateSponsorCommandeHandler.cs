using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Application.SponsorCQRS.Commandes
{
    public class UpdateSponsorCommandeHandler : IRequestHandler<UpdateSponsorQueryRequest, SponsorUpdateDto>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UpdateSponsorCommandeHandler(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<SponsorUpdateDto> Handle(UpdateSponsorQueryRequest request, CancellationToken cancellationToken)
        {
            var existingSponsor = await _unitOfWork.Sponsor.GetByIdAsync(request.Id);

            if (existingSponsor == null)
            {
                return null;
            }
            var ImagName = $"Img_Sponsor_{existingSponsor.SponsorId}";
            Console.WriteLine("ImagName ///" + ImagName);
            var ImagExten = Path.GetExtension(request.SponsorUpdateRequest.SponsorImage.FileName);
            // Supprimer l'ancien fichier image s'il existe
            if (!string.IsNullOrEmpty(existingSponsor.ImagesponsorPath))
            {
                System.IO.File.Delete(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot/UplaodSponsorImage/", $"{ImagName}{ImagExten}"));
            }
            string hosturl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";

            // Enregistrer le nouveau fichier image s'il est fourni
            if (request.SponsorUpdateRequest.SponsorImage != null)
            {
                // Enregistrez l'image dans le dossier wwwroot/UplaodSponsorImage
              
                var localeFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot/UplaodSponsorImage/", $"{ImagName}{ImagExten}");

                using (var fileStream = new FileStream(localeFilePath, FileMode.Create))
                {
                    await request.SponsorUpdateRequest.SponsorImage.CopyToAsync(fileStream);
                }

              
                existingSponsor.ImagesponsorPath = $"{hosturl}/uploads/{ImagName}{ImagExten}";


            }

            // Mapper les propriétés du DTO à l'entité existante
            _mapper.Map(request.SponsorUpdateRequest, existingSponsor);

            // Mettre à jour l'entité dans la base de données
            _unitOfWork.Sponsor.Update(existingSponsor);
            await _unitOfWork.SaveAsync();

            // Mapper l'entité mise à jour vers le DTO de réponse
            var updatedSponsorDto = _mapper.Map<SponsorUpdateDto>(existingSponsor);

            return updatedSponsorDto;
        }
    }
}
