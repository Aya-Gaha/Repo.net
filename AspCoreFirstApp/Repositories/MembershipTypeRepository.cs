using AspCoreFirstApp.Models;
using AspCoreFirstApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspCoreFirstApp.Repositories;

public class MembershipTypeRepository : GenericRepository<MembershipType>, IMembershipTypeRepository
{
    private readonly ApplicationDbContext _context;

    public MembershipTypeRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
}
