using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Application.DTO
{
    public class SupportDto
    {
        public Guid Idsuppport { get; set; }
        public string Namesupport { get; set; }
        public IFormFile File { get; set; }

    }
    public class SupportCreateDto
    {
        public string Namesupport { get; set; }
        public IFormFile File { get; set; }

    }

    public class SupportUpdateDto
    {
        public string Namesupport { get; set; }
        public IFormFile File { get; set; }

    }
  

}
