
using AspCoreFirstApp.Models;
using AspCoreFirstApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspCoreFirstApp.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllWithMembershipTypeAsync()
        {
            return await _context.Customers
                .Include(c => c.MembershipType)
                .ToListAsync();
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
    }
}
