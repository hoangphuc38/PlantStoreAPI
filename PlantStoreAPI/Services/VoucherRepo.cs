using Microsoft.EntityFrameworkCore;
using PlantStoreAPI.Data;
using PlantStoreAPI.Model;
using PlantStoreAPI.Repositories;
using PlantStoreAPI.ViewModel;
using System;
using System.Text.RegularExpressions;

namespace PlantStoreAPI.Services
{
    public class VoucherRepo : IVoucherRepo
    {
        public DataContext _context;
        public VoucherRepo(DataContext context)
        {
            _context = context;
        }
        public async Task<List<Voucher>> GetAll()
        {
            var vouchers = await _context.Vouchers.ToListAsync();

            foreach (var voucher in vouchers)
            {
                var voucherType = await _context.VoucherTypes.FindAsync(voucher.VoucherTypeId);

                if (voucherType != null)
                {
                    voucher.VoucherType = voucherType;
                }
            }

            return vouchers;
        }
        public async Task<List<Voucher>> GetAllOfCustomer(string customerID)
        {
            List<Voucher> listVoucher = new List<Voucher>();

            var voucherApplied = await _context.VoucherApplied
                                               .Where(c => c.CustomerID == customerID)
                                               .ToListAsync();
            foreach(var voucher in voucherApplied)
            {
                var detailVoucher = await _context.Vouchers.FindAsync(voucher.VoucherID);

                if (detailVoucher != null && detailVoucher.DateEnd > DateTime.Now)
                {
                    listVoucher.Add(detailVoucher);
                }
            }

            return listVoucher;
        }
        public async Task<Voucher> GetById(string voucherID)
        {
            var voucher = await _context.Vouchers.Where(c => c.ID == voucherID).FirstAsync();

            if (voucher == null)
            {
                throw new KeyNotFoundException();
            }

            var voucherType = await _context.VoucherTypes.FindAsync(voucher.VoucherTypeId);

            if (voucherType == null)
            {
                throw new Exception("Cannot find type of voucher");
            }

            voucher.VoucherType = voucherType;

            return voucher;
        }
        public async Task<Voucher> Add(VoucherVM voucherVM, int voucherTypeID)
        {
            var voucher = new Voucher
            {
                ID = await AutoID(),
                Name = voucherVM.Name,
                DateBegin = voucherVM.DateBegin,
                DateEnd = voucherVM.DateEnd,
                Value = voucherVM.Value,
                VoucherTypeId = voucherTypeID
            };

            var voucherType = await _context.VoucherTypes.FindAsync(voucherTypeID);

            if (voucherType == null)
            {
                throw new Exception("Cannot find this type of voucher");             
            }
            else
            {
                voucher.VoucherType = voucherType;
            }

            var customers = await _context.Customers.ToListAsync();

            foreach (var customer in customers)
            {
                if (await SpecifyTypes(voucherType.VoucherTypeName, customer.CustomerTypeId))
                {
                    _context.VoucherApplied.Add(new VoucherApplied
                    {
                        VoucherID = voucher.ID,
                        CustomerID = customer.ID,
                    });
                }   
            }

            _context.Vouchers.Add(voucher);
            await _context.SaveChangesAsync();
            return voucher;
        }
        public async Task<Voucher> Delete(string voucherID)
        {
            var voucher = await _context.Vouchers.FindAsync(voucherID);

            if (voucher == null)
            {
                throw new KeyNotFoundException("Not found voucher");
            }

            var voucherCustomer = await _context.VoucherApplied
                                                .Where(c => c.VoucherID == voucherID)
                                                .ToListAsync();

            _context.VoucherApplied.RemoveRange(voucherCustomer);
            _context.Vouchers.Remove(voucher);
            await _context.SaveChangesAsync();
            return voucher;
        }
        public async Task<Voucher> Update(string voucherID, VoucherVM voucherVM, int voucherTypeID)
        {
            var voucher = await _context.Vouchers.FindAsync(voucherID);

            if (voucher == null)
            {
                throw new KeyNotFoundException("Not found voucher");
            }

            voucher.Name = voucherVM.Name;
            voucher.DateBegin = voucherVM.DateBegin;
            voucher.DateEnd = voucherVM.DateEnd;
            voucher.Value = voucherVM.Value;
            voucher.VoucherTypeId = voucherTypeID;

            var voucherType = await _context.VoucherTypes.FindAsync(voucherTypeID);

            if (voucherType == null)
            {
                throw new Exception("Cannot find this type of voucher");
            }
            
            voucher.VoucherType = voucherType;

            _context.Vouchers.Update(voucher);
            await _context.SaveChangesAsync();
            return voucher;
        }
        public async Task<List<Voucher>> SearchByName(string name)
        {
            var vouchers = await _context.Vouchers.Where(c => c.Name.ToLower().Contains(name.ToLower()))
                                                  .ToListAsync();
            foreach (var voucher in vouchers)
            {
                var voucherType = await _context.VoucherTypes.FindAsync(voucher.VoucherTypeId);

                if (voucherType == null)
                {
                    voucher.VoucherType = null;
                }
                else
                {
                    voucher.VoucherType = voucherType;
                }
            }

            return vouchers;  
        }
        public async Task<bool> SpecifyTypes(string voucherType, int customerTypeID)
        {
            var customerType = await _context.CustomersTypes.FindAsync(customerTypeID);

            if (customerType == null)
            {
                throw new Exception("Not exist user");
            }

            switch (voucherType)
            {
                case "All":
                    return true;
                default:
                    if (customerType.CustomerTypeName == voucherType) return true;
                    else return false;
            }
        }
        private async Task<string> AutoID()
        {
            var ID = "PK0001";

            var maxID = await _context.Vouchers
                .OrderByDescending(v => v.ID)
                .Select(v => v.ID)
                .FirstOrDefaultAsync();

            if (maxID == null)
            {
                return ID;
            }

            ID = "PK";

            var numeric = Regex.Match(maxID, @"\d+").Value;

            numeric = (int.Parse(numeric) + 1).ToString();

            while (ID.Length + numeric.Length < 6)
            {
                ID += '0';
            }

            return ID + numeric;
        }
    }
}
