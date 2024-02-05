using Application.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.GalleryCQRS.Queries
{
    public class GetByIdGalleryQueryRequest : IRequest<GalleryDto>
    {
        public Guid id { get; set; }

    }
}
