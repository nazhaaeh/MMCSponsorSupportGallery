using Application.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.GalleryCQRS.Commandes
{
    public class CreateGalleryCommandeRequest :IRequest<GalleryCreateDto>
    {
        public GalleryCreateDto GalleryCreateRequest { get; set; }
        public CreateGalleryCommandeRequest( GalleryCreateDto galleryCreateRequest)
        {

            GalleryCreateRequest = galleryCreateRequest;

        }
    }
}
