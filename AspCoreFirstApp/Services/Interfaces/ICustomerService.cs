using AspCoreFirstApp.Models;

namespace AspCoreFirstApp.Services.Interfaces;

public interface ICustomerService
{
    Task<IEnumerable<Customer>> GetAllCustomersAsync();
    Task<IEnumerable<Customer>> GetAllCustomersWithMembershipAsync();
    Task<Customer?> GetCustomerByIdAsync(int id);
    Task<Customer?> GetCustomerByIdWithMembershipAsync(int id);
    Task<IEnumerable<Customer>> GetCustomersWithHighDiscountAsync(int minDiscountRate = 20);
    Task<Dictionary<string, int>> GetCustomerStatsByMembershipTypeAsync();
    Task CreateCustomerAsync(Customer customer);
    Task UpdateCustomerAsync(Customer customer);
    Task DeleteCustomerAsync(int id);
    Task<bool> CustomerExistsAsync(int id);
}
