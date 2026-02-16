using Microsoft.AspNetCore.Mvc;
using AspCoreFirstApp.Models;
using AspCoreFirstApp.Models.ViewModels;
using AspCoreFirstApp.Repositories.Interfaces;
using AspCoreFirstApp.Services.Interfaces;
using X.PagedList;
using X.PagedList.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspCoreFirstApp.Controllers;

public class MovieController : Controller
{
    private readonly IMovieService _movieService;
    private readonly IGenreRepository _genreRepository;
    private readonly IWebHostEnvironment _env;

    public MovieController(IMovieService movieService, IGenreRepository genreRepository, IWebHostEnvironment env)
    {
        _movieService = movieService;
        _genreRepository = genreRepository;
        _env = env;
    }

    public async Task<IActionResult> Index(string sortOrder, int? page)
    {
        ViewData["CurrentSort"] = sortOrder;
        ViewData["NameSort"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

        int pageSize = 5;
        int pageNumber = page ?? 1;

        var (movies, totalCount) = await _movieService.GetPagedMoviesAsync(pageNumber, pageSize, sortOrder);
        
        var pagedList = new StaticPagedList<Movie>(
            movies,
            pageNumber,
            pageSize,
            totalCount
        );

        return View(pagedList);
    }

    public async Task<IActionResult> detailsList(int id)
    {
        var movie = await _movieService.GetMovieByIdAsync(id);
        if (movie == null)
            return NotFound();
        return View(movie);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var movie = await _movieService.GetMovieByIdAsync(id);
        if (movie == null)
            return NotFound();
        return View(movie);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Movie movie)
    {
        if (!ModelState.IsValid)
            return View(movie);

        if (!await _movieService.MovieExistsAsync(movie.Id))
            return NotFound();

        await _movieService.UpdateMovieAsync(movie);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var movie = await _movieService.GetMovieByIdAsync(id);
        if (movie == null)
            return NotFound();

        return View(movie);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (!await _movieService.MovieExistsAsync(id))
            return NotFound();

        await _movieService.DeleteMovieAsync(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await PopulateGenresAsync();
        return View(new MovieVM());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MovieVM vm)
    {
        await PopulateGenresAsync(vm.Movie.GenreId);

        if (!ModelState.IsValid)
            return View(vm);

        await _movieService.CreateMovieAsync(vm.Movie, vm.Photo, _env.WebRootPath);

        return RedirectToAction(nameof(Index));
    }

    // LINQ Query Examples
    public async Task<IActionResult> MoviesByDate()
    {
        var movies = await _movieService.GetMoviesOrderedByDateAsync();
        return View("Index", movies.ToPagedList(1, 10));
    }

    public async Task<IActionResult> MoviesByGenre()
    {
        var groupedMovies = await _movieService.GetMoviesGroupedByGenreAsync();
        return Json(groupedMovies);
    }

    // Legacy actions kept for compatibility
    public IActionResult ByRelease(int month, int year)
    {
        return Content("Test Month " + month + " " + year);
    }

    public IActionResult Details(int id)
    {
        var customer = new Customer
        {
            Id = id,
            Name = "Client" + id
        };
        var movies = new List<Movie>
        {
            new Movie { Id = 1, Name = "Movie 1" },
            new Movie { Id = 2, Name = "Movie 2" }
        };
        var vm = new MovieCustomerViewModel
        {
            Customer = customer,
            Movies = movies
        };
        return View(vm);
    }

    private async Task PopulateGenresAsync(Guid? selectedId = null)
    {
        var genres = await _genreRepository.GetAllAsync();
        ViewBag.Genres = new SelectList(genres, "Id", "Name", selectedId);
    }

}
