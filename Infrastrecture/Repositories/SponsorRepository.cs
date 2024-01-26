using Application.Interfaces;
using Domain.Models;
using Infrastrecture.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastrecture.Repositories
{
    public class SponsorRepository : GenericRepository<Sponsor>, ISponsorRepository
    {
        public SponsorRepository(DbContextApplication dbcontxt) : base(dbcontxt)
        {
        }
    }
}
