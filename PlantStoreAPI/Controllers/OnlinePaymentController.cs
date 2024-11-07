using Microsoft.AspNetCore.Mvc;
using PlantStoreAPI.Repositories.Utils;
using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OnlinePaymentController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;

        public OnlinePaymentController(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }

        [HttpPost("online-payment/url")]
        public async Task<IActionResult> CreatePaymentVnPayUrl(PaymentRequest paymentRequest)
        {
            string url = await _vnPayService.CreatePaymentUrl(paymentRequest, HttpContext);

            return Ok(url);
        }

        [HttpGet("{orderId}/online-payment/response")]
        public async Task<IActionResult> GetPaymentResponse(string orderId)
        {
            PaymentMethodVM paymentMethodVM;
            IQueryCollection queryCollection = HttpContext.Request.Query;

            paymentMethodVM = await _vnPayService.PaymentExecuteAsync(orderId, queryCollection);
      
            return Ok(paymentMethodVM);
        }
    }
}
