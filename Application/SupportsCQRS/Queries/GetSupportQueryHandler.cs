using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SupportsCQRS.Queries
{
    public class GetSupportQueryHandler : IRequestHandler<GetSupportQueryRequest, List<SupportDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetSupportQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
         

        }
        public async Task<List<SupportDto>> Handle(GetSupportQueryRequest request, CancellationToken cancellationToken)
        {
            var supports = await _unitOfWork.Support.GetAllAsync();
            return _mapper.Map<List<SupportDto>>(supports);
            
        }
    }
}
