using AutoMapper;
using Souq.Api.Dtos;
using Souq.Core.Entites.Products;

namespace Souq.Api.Helper.Resolver
{
    public class PictureUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration _configuration;

        public PictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.PictureUrl))
            return $"{_configuration["BaseUrlApi"]}{source.PictureUrl}";
            return string.Empty;
        }
    }
}
