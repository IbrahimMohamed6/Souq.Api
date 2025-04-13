using Souq.Core.Entites.Products;
using Souq.Core.Specification;

namespace Souq.Core.RepositoryContract
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
      public Task<IReadOnlyList<T>> GetAllAsync();

      public Task<T?> GetByIdAsync(int id);


        public Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> Spec);

        public Task<T?> GetByIdWithSpecAsync(ISpecification<T> Spec);

        public Task<int> GetCountForPagination(ISpecification<T> Spec);

        Task AddAsync(T Item);
        void Delete(T Item);
        void Update(T Item);
        
    }
}
