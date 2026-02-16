using AspCoreFirstApp.Models;
using AspCoreFirstApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspCoreFirstApp.Repositories;

public class MembershipTypeRepository : IMembershipTypeRepository
{
    private readonly ApplicationDbContext _context;

    public MembershipTypeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MembershipType>> GetAllAsync()
    {
        return await _context.MembershipTypes
            .OrderBy(mt => mt.Id)
            .ToListAsync();
    }

    public async Task<MembershipType?> GetByIdAsync(int id)
    {
        return await _context.MembershipTypes.FindAsync(id);
    }
}
