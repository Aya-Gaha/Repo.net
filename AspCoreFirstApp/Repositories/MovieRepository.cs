using AspCoreFirstApp.Models;
using AspCoreFirstApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspCoreFirstApp.Repositories;

public class MovieRepository : GenericRepository<Movie>, IMovieRepository
{
    private readonly ApplicationDbContext _context;

    public MovieRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Movie>> GetAllWithGenreAsync()
    {
        return await _context.Movies!
            .Include(m => m.Genre)
            .ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetPagedWithGenreAsync(int pageNumber, int pageSize, string? sortOrder = null)
    {
        var query = _context.Movies!.Include(m => m.Genre).AsQueryable();

        query = sortOrder?.ToLower() == "name_desc"
            ? query.OrderByDescending(m => m.Name)
            : query.OrderBy(m => m.Name);

        return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Movies!.CountAsync();
    }

    public async Task<Movie?> GetByIdWithGenreAsync(int id)
    {
        return await _context.Movies!
            .Include(m => m.Genre)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Movie>> GetMoviesOrderedByDateAsync()
    {
        return await _context.Movies!
            .Include(m => m.Genre)
            .OrderByDescending(m => m.DateAjoutMovie)
            .ToListAsync();
    }

    public async Task<Dictionary<string, List<Movie>>> GetMoviesGroupedByGenreAsync()
    {
        return await _context.Movies!
            .Include(m => m.Genre)
            .GroupBy(m => m.Genre!.Name)
            .ToDictionaryAsync(
                g => g.Key,
                g => g.ToList()
            );
    }
}
