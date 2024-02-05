using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Support
    {

        public Guid SupportId { get; set; }

        public string Namesupport { get; set; }
        public string FilePath { get; set; }

        public Guid SessionId { get; set; }
    }
}
