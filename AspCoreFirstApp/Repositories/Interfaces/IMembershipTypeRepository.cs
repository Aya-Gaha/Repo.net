using AspCoreFirstApp.Models;

namespace AspCoreFirstApp.Repositories.Interfaces;

public interface IMembershipTypeRepository
{
    Task<IEnumerable<MembershipType>> GetAllAsync();
    Task<MembershipType?> GetByIdAsync(int id);
}
