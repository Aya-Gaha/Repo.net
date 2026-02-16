using AspCoreFirstApp.Models;
using AspCoreFirstApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspCoreFirstApp.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly ApplicationDbContext _context;

    public GenreRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Genre>> GetAllAsync()
    {
        return await _context.Genres
            .OrderBy(g => g.Name)
            .ToListAsync();
    }

    public async Task<Genre?> GetByIdAsync(Guid id)
    {
        return await _context.Genres.FindAsync(id);
    }
}
