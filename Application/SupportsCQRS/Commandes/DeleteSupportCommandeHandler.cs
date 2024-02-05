using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SupportsCQRS.Commandes
{
    public class DeleteSupportCommandeHandler : IRequestHandler<DeleteSupportCommandeRequest>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSupportCommandeHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;


        }
        public async Task Handle(DeleteSupportCommandeRequest request, CancellationToken cancellationToken)
        {
            var existingSupport = await _unitOfWork.Support.GetByIdAsync(request.Id);

            if (existingSupport != null)
            {
                // Supprimer l'ancien fichier PDF s'il existe
                if (!string.IsNullOrEmpty(existingSupport.FilePath))
                {
                    System.IO.File.Delete(existingSupport.FilePath);
                }

                // Supprimer l'entité dans la base de données
                _unitOfWork.Support.Remove(existingSupport);
                await _unitOfWork.SaveAsync();
            }

        }
    }
}
