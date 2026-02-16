using AspCoreFirstApp.Models;

namespace AspCoreFirstApp.Repositories.Interfaces;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<IEnumerable<Customer>> GetAllWithMembershipTypeAsync();
    Task<Customer?> GetByIdAsync(int id);
    Task<Customer?> GetByIdWithMembershipTypeAsync(int id);
    Task<IEnumerable<Customer>> GetCustomersWithHighDiscountAsync(int minDiscountRate);
    Task<Dictionary<string, int>> GetCustomerCountByMembershipTypeAsync();
    Task AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
