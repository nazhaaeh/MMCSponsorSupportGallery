using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.GalleryCQRS.Commandes
{
    public class DeleteGalleryCommandeHandler : IRequestHandler<DeleteGalleryCommandeRequest>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteGalleryCommandeHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;


        }
        public async Task Handle(DeleteGalleryCommandeRequest request, CancellationToken cancellationToken)
        {
            var existingGallery = await _unitOfWork.Gallery.GetByIdAsync(request.Id);

            if (existingGallery != null)
            {
                // Supprimer l'ancien fichier PDF s'il existe
                if (!string.IsNullOrEmpty(existingGallery.imageGalleryPath))
                {
                    System.IO.File.Delete(existingGallery.imageGalleryPath);
                }

                // Supprimer l'entité dans la base de données
                _unitOfWork.Gallery.Remove(existingGallery);
                await _unitOfWork.SaveAsync();
            }
        }
    }
}
