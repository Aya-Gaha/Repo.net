using AspCoreFirstApp.Models;
using Microsoft.AspNetCore.Http;

namespace AspCoreFirstApp.Services.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetAllMoviesAsync();

        Task<IEnumerable<Movie>> GetAllMoviesWithGenreAsync();

        Task<(IEnumerable<Movie> Movies, int TotalCount)> 
            GetPagedMoviesAsync(int pageNumber, int pageSize, string? sortOrder = null);

        Task<Movie?> GetMovieByIdAsync(int id);

        Task<Movie?> GetMovieByIdWithGenreAsync(int id);

        Task<IEnumerable<Movie>> GetMoviesOrderedByDateAsync();

        Task<Dictionary<string, List<Movie>>> GetMoviesGroupedByGenreAsync();

        Task CreateMovieAsync(Movie movie, IFormFile? photoFile, string webRootPath);

        Task UpdateMovieAsync(Movie movie);

        Task DeleteMovieAsync(int id);

        Task<bool> MovieExistsAsync(int id);
    }
}