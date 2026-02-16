using AspCoreFirstApp.Models;

namespace AspCoreFirstApp.Repositories.Interfaces;

public interface IMovieRepository : IGenericRepository<Movie>
{
    Task<IEnumerable<Movie>> GetAllWithGenreAsync();
    Task<IEnumerable<Movie>> GetPagedWithGenreAsync(int pageNumber, int pageSize, string? sortOrder = null);
    Task<int> GetTotalCountAsync();
    Task<Movie?> GetByIdWithGenreAsync(int id);
    Task<IEnumerable<Movie>> GetMoviesOrderedByDateAsync();
    Task<Dictionary<string, List<Movie>>> GetMoviesGroupedByGenreAsync();
}
