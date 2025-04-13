using AutoMapper;
using Souq.Api.Dtos;
using Souq.Core.Entites.Products;

namespace Souq.Api.Helper.Resolver
{
    public class PictureWithBrandAndCategoryResolver : IValueResolver<Product, ProductWithBrandAndCategoryToReturnDto, string>
    {
        private readonly IConfiguration _configuration;

        public PictureWithBrandAndCategoryResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductWithBrandAndCategoryToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
                return $"{_configuration["BaseUrlApi"]}{source.PictureUrl}";
            return string.Empty;
        }
    }
}
