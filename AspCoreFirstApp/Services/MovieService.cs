using AspCoreFirstApp.Models;
using AspCoreFirstApp.Models.ViewModels;
using AspCoreFirstApp.Repositories.Interfaces;
using AspCoreFirstApp.Services.Interfaces;

namespace AspCoreFirstApp.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;

    public MovieService(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
    {
        return await _movieRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Movie>> GetAllMoviesWithGenreAsync()
    {
        return await _movieRepository.GetAllWithGenreAsync();
    }

    public async Task<(IEnumerable<Movie> Movies, int TotalCount)> GetPagedMoviesAsync(int pageNumber, int pageSize, string? sortOrder = null)
    {
        var movies = await _movieRepository.GetPagedWithGenreAsync(pageNumber, pageSize, sortOrder);
        var totalCount = await _movieRepository.GetTotalCountAsync();
        return (movies, totalCount);
    }

    public async Task<Movie?> GetMovieByIdAsync(int id)
    {
        return await _movieRepository.GetByIdAsync(id);
    }

    public async Task<Movie?> GetMovieByIdWithGenreAsync(int id)
    {
        return await _movieRepository.GetByIdWithGenreAsync(id);
    }

    public async Task<IEnumerable<Movie>> GetMoviesOrderedByDateAsync()
    {
        return await _movieRepository.GetMoviesOrderedByDateAsync();
    }

    public async Task<Dictionary<string, List<Movie>>> GetMoviesGroupedByGenreAsync()
    {
        return await _movieRepository.GetMoviesGroupedByGenreAsync();
    }

    public async Task CreateMovieAsync(Movie movie, IFormFile? photoFile, string webRootPath)
    {
        if (photoFile is { Length: > 0 })
        {
            var imagesDir = Path.Combine(webRootPath, "images");
            Directory.CreateDirectory(imagesDir);

            var ext = Path.GetExtension(photoFile.FileName);
            var fileName = $"movie_{Guid.NewGuid():N}{ext}";
            var filePath = Path.Combine(imagesDir, fileName);

            await using var stream = File.Create(filePath);
            await photoFile.CopyToAsync(stream);

            movie.ImageFile = fileName;
        }

        movie.DateAjoutMovie ??= DateTime.UtcNow;

        await _movieRepository.AddAsync(movie);
    }

    public async Task UpdateMovieAsync(Movie movie)
    {
        await _movieRepository.UpdateAsync(movie);
    }

    public async Task DeleteMovieAsync(int id)
    {
        await _movieRepository.DeleteAsync(id);
    }

    public async Task<bool> MovieExistsAsync(int id)
    {
        return await _movieRepository.ExistsAsync(id);
    }
}
