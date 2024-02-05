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
    public class GetGalleryQueryHandler : IRequestHandler<GetGalleryQueryRequest,List<GalleryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetGalleryQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<GalleryDto>> Handle(GetGalleryQueryRequest request, CancellationToken cancellationToken)
        {
            var gallery = await _unitOfWork.Gallery.GetAllAsync();
            return _mapper.Map<List<GalleryDto>>(gallery);
        }
    }
}
