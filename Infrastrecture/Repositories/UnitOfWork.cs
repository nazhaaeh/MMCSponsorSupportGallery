using Application.Interfaces;
using Infrastrecture.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastrecture.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public DbContextApplication _dbcontext;
        public UnitOfWork(DbContextApplication dbcontext)
        {

            _dbcontext = dbcontext;
            Sponsor = new SponsorRepository(dbcontext);
            Support = new SupportRepository(dbcontext);
            Gallery = new GalleryRepository(dbcontext);
        }
        public ISponsorRepository Sponsor { get;  set; }

        public ISupportRepository Support { get; set; }

        public IGalleryRepository Gallery { get; set; }

        public void Dispose()
        {
            _dbcontext.Dispose();
        }

        public async Task<int> SaveAsync()
        {
            return await _dbcontext.SaveChangesAsync();
        }
    }
}
