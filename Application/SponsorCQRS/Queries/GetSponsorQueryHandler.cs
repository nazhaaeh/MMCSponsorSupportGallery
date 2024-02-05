using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SponsorCQRS.Queries
{
    public class GetSponsorQueryHandler : IRequestHandler<GetSponsorQueryRequest, List<SponsorDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetSponsorQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;


        }
        public async Task<List<SponsorDto>> Handle(GetSponsorQueryRequest request, CancellationToken cancellationToken)
        {
            var Sponsor = await _unitOfWork.Sponsor.GetAllAsync();
            return _mapper.Map<List<SponsorDto>>(Sponsor);
        }
    }
}
