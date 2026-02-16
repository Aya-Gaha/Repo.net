using AspCoreFirstApp.Models;
using AspCoreFirstApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspCoreFirstApp.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly ApplicationDbContext _context;

    public MovieRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Movie>> GetAllAsync()
    {
        return await _context.Movies!.ToListAsync();
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

    public async Task<Movie?> GetByIdAsync(int id)
    {
        return await _context.Movies!.FindAsync(id);
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

    public async Task AddAsync(Movie movie)
    {
        await _context.Movies!.AddAsync(movie);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Movie movie)
    {
        var existingMovie = await _context.Movies!.FindAsync(movie.Id);
        if (existingMovie != null)
        {
            existingMovie.Name = movie.Name;
            existingMovie.GenreId = movie.GenreId;
            existingMovie.ImageFile = movie.ImageFile;
            existingMovie.DateAjoutMovie = movie.DateAjoutMovie;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var movie = await _context.Movies!.FindAsync(id);
        if (movie != null)
        {
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Movies!.AnyAsync(m => m.Id == id);
    }
}
