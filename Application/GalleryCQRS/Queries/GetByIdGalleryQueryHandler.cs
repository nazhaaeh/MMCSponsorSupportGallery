using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.GalleryCQRS.Queries
{
    public class GetByIdGalleryQueryHandler : IRequestHandler<GetByIdGalleryQueryRequest, GalleryDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetByIdGalleryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<GalleryDto> Handle(GetByIdGalleryQueryRequest request, CancellationToken cancellationToken)
        {
            var gallery = await _unitOfWork.Gallery.GetByIdAsync(request.id);

            if (gallery == null)
            {
                return null;
            }

            var gallreyDto = _mapper.Map<GalleryDto>(gallery);
            return (gallreyDto) ;
        }
    }
}
