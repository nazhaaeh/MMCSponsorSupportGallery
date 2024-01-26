using Application.DTO;
using AutoMapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
    
            CreateMap<Support, SupportDto>().ReverseMap();
            CreateMap<Support, SupportCreateDto>().ReverseMap();
            CreateMap<Support, SupportUpdateDto>().ReverseMap();



            CreateMap<Sponsor, SponsorDto>().ReverseMap();
            CreateMap<Sponsor, SponsorCreateDto>().ReverseMap();
            CreateMap<Sponsor, SponsorUpdateDto>().ReverseMap();

            CreateMap<Gallery, GalleryDto>().ReverseMap();
            CreateMap<Gallery, GalleryCreateDto>().ReverseMap();
            CreateMap<Gallery, GalleryUpdateDto>().ReverseMap();


        }
    }
}
