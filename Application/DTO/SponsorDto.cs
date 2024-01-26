using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
   public class SponsorDto
    {
        public Guid Idsponsor { get; set; }
        public string Namesponsor { get; set; }
        public IFormFile SponsorImage { get; set; }
    }
    public class SponsorCreateDto
    {
        public string Namesponsor { get; set; }
        public IFormFile SponsorImage { get; set; }


    }

    public class SponsorUpdateDto
    {
        public string Namesponsor { get; set; }
        public IFormFile SponsorImage { get; set; }

    }
}
