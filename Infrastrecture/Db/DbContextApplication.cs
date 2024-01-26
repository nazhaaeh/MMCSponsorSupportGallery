using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastrecture.Db
{
    public class DbContextApplication: DbContext
    {
        public DbContextApplication(DbContextOptions<DbContextApplication>options):base(options)
        {
            
        }
        public DbSet<Sponsor> Sponsors { get; set;}
        public DbSet<Support> Supports { get; set; }
        public DbSet<Gallery> Gallerys { get; set; }
        public DbSet<SessionSponsor> SessionSponsors { get;set; }


    }
}
