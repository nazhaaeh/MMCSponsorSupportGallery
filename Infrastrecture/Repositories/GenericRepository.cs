using Application.Interfaces;
using Infrastrecture.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastrecture.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbContextApplication _dbcontxt;
      
        public GenericRepository(DbContextApplication dbcontxt)
        {
         _dbcontxt = dbcontxt;

        }

        public async Task AddAsync(T entity)
        {
            await _dbcontxt.Set<T>().AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbcontxt.Set<T>().AddRangeAsync(entities);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbcontxt.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbcontxt.Set<T>().ToListAsync();
           
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbcontxt.Set<T>().FindAsync(id);
        }

        public void Remove(T entity)
        {
            _dbcontxt.Set<T>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbcontxt.Set<T>().RemoveRange(entities);
        }

        public void Update(T entity)
        {
            _dbcontxt.Entry(entity).State = EntityState.Modified;
           
        }
    }
}
