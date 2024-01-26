using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Sponsor
    {
        public Guid SponsorId { get; set; }
        public string Namesponsor { get; set; }
        public string ImagesponsorPath { get; set; }
        public Guid SessionId { get; set; }

    }
}
