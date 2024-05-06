using PlantStoreAPI.Data;
using PlantStoreAPI.Model;
using PlantStoreAPI.Repositories;
using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.Services
{
    public class CustomerRepo : ICustomerRepo
    {
        public DataContext _context;
        public CustomerRepo(DataContext context)
        {
            _context = context;
        }
        public async Task<Customer> GetInfo(string customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);

            if (customer == null)
            {
                throw new KeyNotFoundException(customerId);
            }

            return customer;
        }
        public async Task<Customer> UpdateInfo(string customerId, CustomerVM customerVM)
        {
            var customer = await _context.Customers.FindAsync(customerId);

            if (customer == null)
            {
                throw new KeyNotFoundException(customerId);
            }

            customer.Name = customerVM.Name;
            customer.Address = customerVM.Address;
            customer.Phone = customerVM.Phone;
            customer.DateBirth = customerVM.DateBirth;
            customer.Male = customerVM.Male;
            customer.Avatar = "https://static.vecteezy.com/system/resources/previews/009/292/244/original/default-avatar-icon-of-social-media-user-vector.jpg";

            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();

            return customer;
        }
    }
}
