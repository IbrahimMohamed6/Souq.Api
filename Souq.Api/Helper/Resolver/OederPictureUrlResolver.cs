using AutoMapper;
using AutoMapper.Execution;
using Souq.Api.Dtos.Order;
using Souq.Core.Entites.OrderAggregate;

namespace Souq.Api.Helper.Resolver
{
    public class OederPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;

        public OederPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.PictureUrl))
            {
                return $"{_configuration["BaseUrlApi"]}{source.Product.PictureUrl}";
                 
            }
            return string.Empty;
        }
    }
}
