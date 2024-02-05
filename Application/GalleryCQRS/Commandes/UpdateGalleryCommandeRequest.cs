using Application.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.GalleryCQRS.Commandes
{
  public class UpdateGalleryCommandeRequest: IRequest<GalleryUpdateDto>
    {
        public Guid GalleryId { get; set; }
        public GalleryUpdateDto GalleryUpdateRequest { get; set; }
    }
}
