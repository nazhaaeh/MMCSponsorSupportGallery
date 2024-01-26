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
    public class SupportRepository : GenericRepository<Support>, ISupportRepository
    {
        public SupportRepository(DbContextApplication dbcontxt) : base(dbcontxt)
        {
        }
    }
}
