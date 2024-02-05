using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.GalleryCQRS.Commandes
{
    public class DeleteGalleryCommandeRequest :IRequest
    {
        public Guid Id { get; set; }
    }
}
