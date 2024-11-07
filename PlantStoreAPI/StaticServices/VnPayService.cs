using Microsoft.EntityFrameworkCore;
using PlantStoreAPI.Data;
using PlantStoreAPI.Helpers;
using PlantStoreAPI.Model;
using PlantStoreAPI.Repositories.Utils;
using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.StaticServices
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _config;
        private readonly DataContext _context;

        public VnPayService(IConfiguration config, DataContext context)
        {
            _config = config;
            _context = context;
        }

        public async Task<string> CreatePaymentUrl(PaymentRequest paymentRequest, HttpContext httpContext)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == paymentRequest.UserId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_config["VnPay:TimeZoneId"]);

            var tick = DateTime.Now.Ticks.ToString();
            var vnPayLibrary = new VnPayLibrary();
            var urlCallBack = _config["VnPay:ReturnUrl"];

            vnPayLibrary.AddRequestData("vnp_Version", _config["VnPay:Version"]);
            vnPayLibrary.AddRequestData("vnp_Command", _config["VnPay:Command"]);
            vnPayLibrary.AddRequestData("vnp_TmnCode", _config["VnPay:TmnCode"]);
            vnPayLibrary.AddRequestData("vnp_Amount", ((int)paymentRequest.OrderTotal * 100 * 22000).ToString());
            vnPayLibrary.AddRequestData("vnp_CreateDate", paymentRequest.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnPayLibrary.AddRequestData("vnp_CurrCode", _config["VnPay:CurrCode"]);
            vnPayLibrary.AddRequestData("vnp_IpAddr", vnPayLibrary.GetIpAddress(httpContext));
            vnPayLibrary.AddRequestData("vnp_Locale", _config["VnPay:Locale"]);
            vnPayLibrary.AddRequestData("vnp_OrderInfo", $"Mã khách hàng: {user.UserName}\n " +
                $"Thanh toán đơn hàng: {paymentRequest.OrderId}\n "  +
                $"Tổng cộng: {paymentRequest.OrderTotal}$");
            vnPayLibrary.AddRequestData("vnp_OrderType", "other");
            vnPayLibrary.AddRequestData("vnp_ReturnUrl", urlCallBack);
            vnPayLibrary.AddRequestData("vnp_TxnRef", tick);

            var paymentUrl = vnPayLibrary.CreateRequestUrl(_config["VnPay:BaseUrl"], _config["VnPay:HashSecret"]);

            return paymentUrl;
        }

        public async Task<PaymentMethodVM> PaymentExecuteAsync(string orderId, IQueryCollection collections)
        {
            // get payment method entity
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
            {
                throw new Exception("Not found order");
            }

            var paymentMethod = await _context.PaymentMethods.FindAsync(order.PaymentMethodId);

            if (paymentMethod == null)
            {
                throw new Exception("Not found payment method");
            }

            var paymentMethodVM = new PaymentMethodVM
            {
                PaymentMethodId = paymentMethod.PaymentMethodId,
                PaymentTransactionNo = paymentMethod.PaymentTransactionNo,
                PaymentProvider = paymentMethod.PaymentProvider,
                PaymentCartType = "other",
                PaymentDate = paymentMethod.PaymentDate,
                PaymentStatus = paymentMethod.PaymentStatus,
                IsDefault = paymentMethod.IsDefault,
                PaymentDescription = paymentMethod.PaymentDescription,
                PaymentTypeId = paymentMethod.PaymentTypeId,
            };

            var vnPayLibrary = new VnPayLibrary();

            var dataResponse = vnPayLibrary.GetFullResponseData(paymentMethodVM, collections, _config["VnPay:HashSecret"]);

            paymentMethod.PaymentTransactionNo = dataResponse.PaymentTransactionNo;
            paymentMethod.PaymentProvider = dataResponse.PaymentProvider;
            paymentMethod.PaymentCartType = dataResponse.PaymentCartType;
            paymentMethod.PaymentDate = dataResponse.PaymentDate;
            paymentMethod.PaymentStatus = dataResponse.PaymentStatus;
            paymentMethod.PaymentDescription = dataResponse.PaymentDescription;

            order.IsPaid = true;
            _context.Orders.Update(order);
            _context.PaymentMethods.Update(paymentMethod);

            await _context.SaveChangesAsync();

            var paymentMethodVMResult = new PaymentMethodVM
            {
                PaymentMethodId = paymentMethod.PaymentMethodId,
                PaymentTransactionNo = dataResponse.PaymentTransactionNo,
                PaymentProvider = dataResponse.PaymentProvider,
                PaymentCartType = "other",
                PaymentDate = dataResponse.PaymentDate,
                PaymentStatus = dataResponse.PaymentStatus,
                IsDefault = paymentMethod.IsDefault,
                PaymentDescription = dataResponse.PaymentDescription,
                PaymentTypeId = paymentMethod.PaymentTypeId,
            };

            return paymentMethodVMResult;
        }
    }
}
