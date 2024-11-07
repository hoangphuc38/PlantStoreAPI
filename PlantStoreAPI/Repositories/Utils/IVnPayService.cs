using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.Repositories.Utils
{
    public interface IVnPayService
    {
        Task<string> CreatePaymentUrl(PaymentRequest model, HttpContext context);
        Task<PaymentMethodVM> PaymentExecuteAsync(string orderId, IQueryCollection collections);
    }
}
