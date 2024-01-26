using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class SessionSponsor
    {
        public Guid SessionSponsorId { get; set; }
        public Guid SponsorId { get; set; }
        public Guid SessionId { get; set; }
        public virtual Sponsor Sponsor { get; set; }

    }
}
