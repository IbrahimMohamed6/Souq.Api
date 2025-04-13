using Microsoft.EntityFrameworkCore;
using Souq.Core.Entites.Products;
using Souq.Core.RepositoryContract;
using Souq.Core.Specification;
using Souq.Infrastructure.Data.DbContexts;
using Souq.Infrastructure.Specifications;


namespace Souq.Infrastructure.Repository
{
    public class Genericrepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public Genericrepository(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()

          => await _dbContext.Set<T>().ToListAsync();


        public async Task<T?> GetByIdAsync(int id)

           => await _dbContext.Set<T>().FirstAsync(x => x.Id == id);



        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> Spec)

          => await ApplySpecification(Spec).AsNoTracking().ToListAsync();

        public async Task<T?> GetByIdWithSpecAsync(ISpecification<T> Spec)

           => await ApplySpecification(Spec).FirstOrDefaultAsync();

        private IQueryable<T> ApplySpecification(ISpecification<T> Spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), Spec);
        }

        public Task<int> GetCountForPagination(ISpecification<T> Spec)
        {
            return ApplySpecification(Spec).CountAsync();
        }

        public async Task AddAsync(T Item)
       => await _dbContext.Set<T>().AddAsync(Item);

        public void Delete(T Item)
        => _dbContext.Set<T>().Remove(Item);

        public void Update(T Item)
        => _dbContext.Set<T>().Update(Item);
    }
}
