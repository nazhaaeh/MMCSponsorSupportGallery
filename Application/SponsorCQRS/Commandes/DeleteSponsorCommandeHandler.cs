using Application.Interfaces;
using MediatR;
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

        public DeleteSponsorCommandeHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;


        }
        public async Task Handle(DeleteSponsorCommandeRequest request, CancellationToken cancellationToken)
        {
            var existingSponsor = await _unitOfWork.Sponsor.GetByIdAsync(request.Id);

            if (existingSponsor != null)
            {
                // Supprimer l'ancien fichier PDF s'il existe
                if (!string.IsNullOrEmpty(existingSponsor.ImagesponsorPath))
                {
                    System.IO.File.Delete(existingSponsor.ImagesponsorPath);
                }

                // Supprimer l'entité dans la base de données
                _unitOfWork.Sponsor.Remove(existingSponsor);
                await _unitOfWork.SaveAsync();
            }

        }
    }
}
