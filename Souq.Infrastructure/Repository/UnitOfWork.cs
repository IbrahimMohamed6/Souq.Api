using Microsoft.EntityFrameworkCore;
using Souq.Core.Entites.Products;
using Souq.Core.RepositoryContract;
using Souq.Infrastructure.Data.DbContexts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Souq.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _context;
        private readonly Hashtable _repositories = new();


        public UnitOfWork(StoreContext context)
        {
            _context = context;
        }
        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryInstance = new Genericrepository<T>(_context);
                _repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<T>)_repositories[type]!;
        }

        public async Task<int> SaveChangesAsyc()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Database update error: {ex.InnerException?.Message}", ex);
            }
        }
        
        public async ValueTask DisposeAsync()
        => await _context.DisposeAsync();
    }
}
