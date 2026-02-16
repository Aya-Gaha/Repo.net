using AspCoreFirstApp.Models;
using AspCoreFirstApp.Repositories.Interfaces;
using AspCoreFirstApp.Services.Interfaces;

namespace AspCoreFirstApp.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        return await _customerRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Customer>> GetAllCustomersWithMembershipAsync()
    {
        return await _customerRepository.GetAllWithMembershipTypeAsync();
    }

    public async Task<Customer?> GetCustomerByIdAsync(int id)
    {
        return await _customerRepository.GetByIdAsync(id);
    }

    public async Task<Customer?> GetCustomerByIdWithMembershipAsync(int id)
    {
        return await _customerRepository.GetByIdWithMembershipTypeAsync(id);
    }

    public async Task<IEnumerable<Customer>> GetCustomersWithHighDiscountAsync(int minDiscountRate = 20)
    {
        return await _customerRepository.GetCustomersWithHighDiscountAsync(minDiscountRate);
    }

    public async Task<Dictionary<string, int>> GetCustomerStatsByMembershipTypeAsync()
    {
        return await _customerRepository.GetCustomerCountByMembershipTypeAsync();
    }

    public async Task CreateCustomerAsync(Customer customer)
    {
        await _customerRepository.AddAsync(customer);
        await _customerRepository.SaveChangesAsync();
    }

    public async Task UpdateCustomerAsync(Customer customer)
    {
        _customerRepository.Update(customer);
        await _customerRepository.SaveChangesAsync();
    }

    public async Task DeleteCustomerAsync(int id)
    {
        await _customerRepository.DeleteAsync(id);
        await _customerRepository.SaveChangesAsync();
    }

    public async Task<bool> CustomerExistsAsync(int id)
    {
        return await _customerRepository.ExistsAsync(id);
    }
}
