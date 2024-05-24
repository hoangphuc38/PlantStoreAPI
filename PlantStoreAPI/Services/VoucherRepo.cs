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
            return await _context.Vouchers.ToListAsync();
        }
        public async Task<Voucher> GetById(string voucherID)
        {
            var voucher = await _context.Vouchers.FindAsync(voucherID);

            if (voucher == null)
            {
                throw new KeyNotFoundException();
            }

            return voucher;
        }
        public async Task<Voucher> Add(VoucherVM voucherVM)
        {
            var voucher = new Voucher
            {
                ID = await AutoID(),
                Name = voucherVM.Name,
                DateBegin = voucherVM.DateBegin,
                DateEnd = voucherVM.DateEnd,
                Value = voucherVM.Value,
            };

            var customers = await _context.Customers.ToListAsync();

            foreach (var customer in customers)
            {
                _context.VoucherApplied.Add(new VoucherApplied
                {
                    VoucherID = voucher.ID,
                    CustomerID = customer.ID,
                });
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
        public async Task<Voucher> Update(string voucherID, VoucherVM voucherVM)
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

            _context.Vouchers.Update(voucher);
            await _context.SaveChangesAsync();
            return voucher;
        }
        public async Task<List<Voucher>> SearchByName(string name)
        {
            return await _context.Vouchers.Where(c => c.Name.ToLower().Contains(name.ToLower()))
                                                  .ToListAsync();           
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
