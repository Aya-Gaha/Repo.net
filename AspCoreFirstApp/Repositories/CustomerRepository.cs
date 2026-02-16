
using AspCoreFirstApp.Models;
using AspCoreFirstApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspCoreFirstApp.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<IEnumerable<Customer>> GetAllWithMembershipTypeAsync()
        {
            return await _context.Customers
                .Include(c => c.MembershipType)
                .ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<Customer?> GetByIdWithMembershipTypeAsync(int id)
        {
            return await _context.Customers
                .Include(c => c.MembershipType)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Customer>> GetCustomersWithHighDiscountAsync(int minDiscountRate)
        {
            return await _context.Customers
                .Include(c => c.MembershipType)
                .Where(c => c.MembershipType != null &&
                            c.MembershipType.DiscountRate > minDiscountRate)
                .ToListAsync();
        }

        public async Task<Dictionary<string, int>> GetCustomerCountByMembershipTypeAsync()
        {
            return await _context.Customers
                .Include(c => c.MembershipType)
                .GroupBy(c => c.MembershipType!.DiscountRate)
                .Select(g => new
                {
                    MembershipType = $"{g.Key}% Discount",
                    Count = g.Count()
                })
                .ToDictionaryAsync(x => x.MembershipType, x => x.Count);
        }

        public async Task AddAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Customer customer)
        {
            var existingCustomer = await _context.Customers.FindAsync(customer.Id);

            if (existingCustomer != null)
            {
                existingCustomer.Name = customer.Name;
                existingCustomer.MembershipTypeId = customer.MembershipTypeId;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Customers.AnyAsync(c => c.Id == id);
        }
    }
}
