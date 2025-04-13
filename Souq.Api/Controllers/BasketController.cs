using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Souq.Api.Dtos.Basket;
using Souq.Api.Helper.Errors;
using Souq.Core.Entites.Basket;
using Souq.Core.RepositoryContract;

namespace Souq.Api.Controllers
{
   
    public class BasketController : BaseApiController
    {
        private readonly IBasketReposatory _basketRepo;
        private readonly IMapper _mapper;

        public BasketController(IBasketReposatory basketRepo,
            IMapper mapper)
        {
            _basketRepo = basketRepo;
            _mapper = mapper;
        }

        [HttpGet("{BasketId}")]
        public async Task<ActionResult<CustomerBasket>> Basket(string BasketId)
        {
            var Baskets= await _basketRepo.GetBasket(BasketId);
            if (Baskets == null)
                return new CustomerBasket(BasketId);
            return Ok(Baskets);
                    
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasketToReturnDto>> UpdateCustomer(CustomerBasketToReturnDto basket)
        {
            var MappedBasket = _mapper.Map<CustomerBasketToReturnDto, CustomerBasket>(basket);
            var UpdateOrCreateBaket=await _basketRepo.UpdateBasket(MappedBasket);
            if (UpdateOrCreateBaket is null)
                return BadRequest(new ApiResponse(400));
            return Ok(UpdateOrCreateBaket);

        }
        [HttpDelete("{BasketId}")]
        public async Task<ActionResult<bool>> DeleteBasket(string BasketId)
        {
            return await _basketRepo.DeleteBasket(BasketId);
        }
    }
}
