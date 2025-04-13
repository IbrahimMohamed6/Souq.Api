using Souq.Core.Entites.Products;

namespace Souq.Core.Specification
{
    public class ProductSpecification : BaseSpecification<Product>
    {
        public ProductSpecification(ProductSpecParams spec)
            : base(
                 x =>
            (string.IsNullOrEmpty(spec.Search) || x.Name.Contains(spec.Search)) &&
            (!spec.BrandId.HasValue || x.BrandId == spec.BrandId) &&
            (!spec.CategoryId.HasValue || x.CategoryId == spec.CategoryId)
                 )
        {
            Include.Add(P => P.Brand);
            Include.Add(P => P.Category);

            if (!string.IsNullOrEmpty(spec.Sort))
            {
                switch (spec.Sort)
                {
                    case "priceAsc":
                        AddOrderby(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderbyDesc(p => p.Price);
                        break;
                    default:
                        AddOrderby(p => p.Name);
                        break;
                }
            }
            else
            {
                AddOrderby(p => p.Name);
            }

            ApplyPaging(spec.PageSize * (spec.PageIndex - 1), spec.PageSize);


        }

        public ProductSpecification(int id)
            : base(p => p.Id == id)
        {
            Include.Add(P => P.Brand);
            Include.Add(P => P.Category);
        }
    }
}
