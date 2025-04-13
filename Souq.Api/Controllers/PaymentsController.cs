using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Souq.Api.Dtos.Basket;
using Souq.Api.Helper.Errors;
using Souq.Core.RepositoryContract;
using Souq.Core.Service.Contarct;
using Stripe;

namespace Souq.Api.Controllers
{

    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly IBasketReposatory _basketReposatory;
        private const string StripeSecret = "whsec_5ab4fc13db0c3a0af47e45690d36254d9e44c9b38dfd727184090adb98a2786f";

        public PaymentsController(IPaymentService paymentService
            , IBasketReposatory basketReposatory)
        {
            _paymentService = paymentService;
            _basketReposatory = basketReposatory;
        }
        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasketToReturnDto>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var Basket = await _paymentService.CreateOreUpdatePaymentIntent(basketId);
            if (Basket is null)
                return BadRequest(new ApiResponse(400, "There is Aproblrm in Your Basket"));
            return Ok(Basket);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> HandleStripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], StripeSecret);
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

                if (stripeEvent.Type == "payment_intent.payment_failed")
                {
                    await _paymentService.UpdateOrderStatus(paymentIntent.Id, false);
                }
                else if (stripeEvent.Type == "payment_intent.succeeded")
                {
                    await _paymentService.UpdateOrderStatus(paymentIntent.Id, true);
                }

                return Ok();
            }
            catch (StripeException e)
            {

                return BadRequest($"Webhook error: {e.Message}");
            }
        }

    }

}
