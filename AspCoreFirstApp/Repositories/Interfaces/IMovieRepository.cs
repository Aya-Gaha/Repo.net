using AspCoreFirstApp.Models;

namespace AspCoreFirstApp.Repositories.Interfaces;

public interface IMovieRepository
{
    Task<IEnumerable<Movie>> GetAllAsync();
    Task<IEnumerable<Movie>> GetAllWithGenreAsync();
    Task<IEnumerable<Movie>> GetPagedWithGenreAsync(int pageNumber, int pageSize, string? sortOrder = null);
    Task<int> GetTotalCountAsync();
    Task<Movie?> GetByIdAsync(int id);
    Task<Movie?> GetByIdWithGenreAsync(int id);
    Task<IEnumerable<Movie>> GetMoviesOrderedByDateAsync();
    Task<Dictionary<string, List<Movie>>> GetMoviesGroupedByGenreAsync();
    Task AddAsync(Movie movie);
    Task UpdateAsync(Movie movie);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
