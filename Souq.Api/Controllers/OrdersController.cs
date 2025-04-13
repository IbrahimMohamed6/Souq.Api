using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Souq.Api.Dtos.Identity;
using Souq.Api.Dtos.Order;
using Souq.Api.Helper.Errors;
using Souq.Core.Entites.OrderAggregate;
using Souq.Core.RepositoryContract;
using Souq.Core.Service.Contarct;
using System.Security.Claims;
using Order = Souq.Core.Entites.OrderAggregate.Order;

namespace Souq.Api.Controllers
{

    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService
            ,IUnitOfWork unitOfWork
            , IMapper mapper)
        {
            _orderService = orderService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto model)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var MappedAddress = _mapper.Map<AddressIdentityDto, OrderAddress>(model.shipToAddress);
            var Order = await _orderService.CrateOrderAsync(Email, model.basketId, model.DeliveryMethodId,MappedAddress);
            if (Order is null)
                return BadRequest(new ApiResponse(400, "Error While Crating Order"));
            return Ok(Order);

        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<OrderTorReturnDto>>> GetOrders()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var Orders = await _orderService.GetOrderForSpecificUser(Email);
            if (Orders is null)
                return NotFound(new ApiResponse(404, "Order Not Found"));
           var MappedOrder = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderTorReturnDto>>(Orders);
            return Ok(MappedOrder);
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<OrderTorReturnDto>> GetOrder(int id)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var Order = await _orderService.GetByIdOrderForSpecificUser(Email, id);
            if (Order is null)
                return NotFound(new ApiResponse(404, "Order Not Found"));
            var MappedOrder = _mapper.Map<Order, OrderTorReturnDto>(Order);
            return Ok(MappedOrder);
        }

        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<DeliveryMethod>> GetDeliveryMehod()
        {
            var DeliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return Ok(DeliveryMethods);
        }

    }
}
