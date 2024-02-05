// UpdateSupportCommandeHandler
using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Application.SupportsCQRS.Commandes
{
    public class UpdateSupportCommandeHandler : IRequestHandler<UpdateSupportCommandeRequest, SupportUpdateDto>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UpdateSupportCommandeHandler(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<SupportUpdateDto> Handle(UpdateSupportCommandeRequest request, CancellationToken cancellationToken)
        {
            var existingSupport = await _unitOfWork.Support.GetByIdAsync(request.SupportId);

            if (existingSupport == null)
            {
                return null;
            }
            var ImagName = $"Img_Support_{existingSupport.SupportId}";
            var ImagExten = Path.GetExtension(request.SupportUpdateRequest.File.FileName);
            // Supprimer l'ancien fichier image s'il existe
            if (!string.IsNullOrEmpty(existingSupport.FilePath))
            {
                System.IO.File.Delete(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot/uploads/", $"{ImagName}{ImagExten}"));
            }
            string hosturl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";

            // Enregistrer le nouveau fichier image s'il est fourni
            if (request.SupportUpdateRequest.File != null)
            {
                // Enregistrez l'image dans le dossier wwwroot/UplaodSponsorImage

                var localeFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot/uploads/", $"{ImagName}{ImagExten}");

                using (var fileStream = new FileStream(localeFilePath, FileMode.Create))
                {
                    await request.SupportUpdateRequest.File.CopyToAsync(fileStream);
                }

                  existingSupport.FilePath = $"{hosturl}/uploads/{ImagName}{ImagExten}";
            }

            // Mapper les propriétés du DTO à l'entité existante
            _mapper.Map(request.SupportUpdateRequest, existingSupport);

            // Mettre à jour l'entité dans la base de données
            _unitOfWork.Support.Update(existingSupport);
            await _unitOfWork.SaveAsync();

            // Mapper l'entité mise à jour vers le DTO de réponse
            var updatedSponsorDto = _mapper.Map<SupportUpdateDto>(existingSupport);

            return updatedSponsorDto;
        }
    }
}
