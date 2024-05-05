using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlantStoreAPI.Data;
using PlantStoreAPI.Model;
using PlantStoreAPI.Repositories;
using PlantStoreAPI.Response;
using PlantStoreAPI.ViewModel;
using System.Text.RegularExpressions;

namespace PlantStoreAPI.Services
{
    public class AccountRepo : IAccountRepo
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private SignInManager<AppUser> _signInManager;
        private readonly JWTManger _JWTManager;
        private readonly DataContext _context;

        public AccountRepo(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<AppUser> signInManager,   
            JWTManger JWTManager,
            DataContext context
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _JWTManager = JWTManager;
            _context = context;
        }

        public async Task<Customer> Register(Register register, string role)
        {
            var checkMail = await _userManager.FindByEmailAsync(register.Email);

            if (checkMail != null)
            {
                throw new Exception("Email Exists!");
            }

            var user = new AppUser
            {
                Id = await AutoID(),
                Email = register.Email,
                UserName = register.UserName,
                PhoneNumber = register.Phone,
            };

            var customer = new Customer
            {
                ID = user.Id,
                Name = user.UserName,
                Phone = user.PhoneNumber,
                Address = "",
                DateBirth = DateTime.MinValue,
                Avatar = "https://static.vecteezy.com/system/resources/previews/009/292/244/original/default-avatar-icon-of-social-media-user-vector.jpg"
            };

            _context.Customers.Add(customer);

            await _userManager.CreateAsync(user, register.Password);

            if (!await _roleManager.RoleExistsAsync(role))
            {
                throw new Exception("This Role does not exist");
                
            }

            await _userManager.AddToRoleAsync(user, role);

            await _context.SaveChangesAsync();

            return customer;
        }

        public async Task<LoginResponse> Login(Login login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);

            if (user == null)
            {
                throw new ArgumentNullException("User not found");
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, login.Password);

            if (!passwordValid)
            {
                throw new KeyNotFoundException();
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            return new LoginResponse
            {
                access_token = _JWTManager.GenerateToken(user.Email, userRoles.FirstOrDefault() ?? ""),
                id = user.Id,
                name = user.UserName,
                email = user.Email,
                role = userRoles.FirstOrDefault(),
                Customer = await _context.Customers.FindAsync(user.Id)
            };
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        private async Task<string> AutoID()
        {
            var ID = "CS0001";

            var maxID = await _context.Customers
                .OrderByDescending(v => v.ID)
                .Select(v => v.ID)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(maxID))
            {
                return ID;
            }

            var numeric = Regex.Match(maxID, @"\d+").Value;

            if (string.IsNullOrEmpty(numeric))
            {
                return ID;
            }

            ID = "CS";

            numeric = (int.Parse(numeric) + 1).ToString();

            while (ID.Length + numeric.Length < 6)
            {
                ID += '0';
            }

            return ID + numeric;
        }

    }
}
