using AspCoreFirstApp.Models;

namespace AspCoreFirstApp.Repositories.Interfaces;

public interface ICustomerRepository : IGenericRepository<Customer>
{
    Task<IEnumerable<Customer>> GetAllWithMembershipTypeAsync();
    Task<Customer?> GetByIdWithMembershipTypeAsync(int id);
    Task<IEnumerable<Customer>> GetCustomersWithHighDiscountAsync(int minDiscountRate);
    Task<Dictionary<string, int>> GetCustomerCountByMembershipTypeAsync();
}
