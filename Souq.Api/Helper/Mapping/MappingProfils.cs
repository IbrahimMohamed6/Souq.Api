using AutoMapper;
using Souq.Api.Dtos;
using Souq.Api.Dtos.Basket;
using Souq.Api.Dtos.Identity;
using Souq.Api.Dtos.Order;
using Souq.Api.Helper.Resolver;
using Souq.Core.Entites.Basket;
using Souq.Core.Entites.Identity;
using Souq.Core.Entites.OrderAggregate;
using Souq.Core.Entites.Products;

namespace Souq.Api.Helper.Mapping
{
    public class MappingProfils : Profile
    {
        public MappingProfils()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.Brand, O => O.MapFrom(S => S.Brand.Name))
                .ForMember(d => d.Category, O => O.MapFrom(S => S.Category.Name))
                .ForMember(d => d.PictureUrl, O => O.MapFrom<PictureUrlResolver>());

            CreateMap<UserAddress, AddressIdentityDto>().ReverseMap();
            CreateMap<BasketItemToReturnDto, BasketItem>();
            CreateMap<CustomerBasketToReturnDto, CustomerBasket>();
            CreateMap<OrderAddress, AddressIdentityDto>().ReverseMap();
            CreateMap<Order, OrderTorReturnDto>()
                .ForMember(o => o.Status, o => o.MapFrom(o => o.Status))
                .ForMember(o => o.DeliveryMethod, o => o.MapFrom(o => o.DeliveryMethod.ShortName))
                .ForMember(o => o.DeliveryMethodCost, o => o.MapFrom(o => o.DeliveryMethod.Cost));
                

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(i => i.ProductId, i => i.MapFrom(i => i.Product.ProductId))
                .ForMember(i => i.ProductName, i => i.MapFrom(i => i.Product.ProductName))
                .ForMember(i => i.PictureUrl, i => i.MapFrom(i => i.Product.PictureUrl))
                .ForMember(i => i.PictureUrl, i => i.MapFrom<OederPictureUrlResolver>());


            CreateMap<Product, ProductWithBrandAndCategoryToReturnDto>()
                .ForMember(d => d.PictureUrl, O => O.MapFrom<PictureWithBrandAndCategoryResolver>());
        }
    }
}
