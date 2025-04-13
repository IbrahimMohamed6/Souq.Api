

using Microsoft.EntityFrameworkCore;
using Souq.Core.Entites.Products;
using Souq.Core.Specification;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Souq.Infrastructure.Specifications
{
    public static class SpecificationEvaluator<TEntity>where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> InputQuery,ISpecification<TEntity> Spec)
        {
            var Query = InputQuery;
            if (Spec.Criteria is not null)
                Query = Query.Where(Spec.Criteria);
            if (Spec.Orderby is not null)
                 Query = Query.OrderBy(Spec.Orderby);
            if (Spec.OrderbyDesc is not null)
                Query = Query.OrderByDescending(Spec.OrderbyDesc);

            if (Spec.IsPaginated)
              Query = Query.Skip(Spec.Skip).Take(Spec.Take);
            Query = Spec.Include.Aggregate(Query, (CurrentQuery, IncludeExpression) => CurrentQuery.Include(IncludeExpression));
            

            return Query;
            
        }
    }
}
