using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.SponsorCQRS.Commandes
{
    public class DeleteSponsorCommandeHandler : IRequestHandler<DeleteSponsorCommandeRequest>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DeleteSponsorCommandeHandler(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task Handle(DeleteSponsorCommandeRequest request, CancellationToken cancellationToken)
        {
            var existingSponsor = await _unitOfWork.Sponsor.GetByIdAsync(request.Id);

            if (existingSponsor != null)
            {
                // Supprimer l'ancien fichier PDF s'il existe
                if (!string.IsNullOrEmpty(existingSponsor.ImagesponsorPath))
                {
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, existingSponsor.ImagesponsorPath);

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                // Supprimer l'entité dans la base de données
                _unitOfWork.Sponsor.Remove(existingSponsor);
                await _unitOfWork.SaveAsync();
            }
        }
    }
}

