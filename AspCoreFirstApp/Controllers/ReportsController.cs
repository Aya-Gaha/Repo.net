using AspCoreFirstApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AspCoreFirstApp.Controllers;

/// <summary>
/// Controller démontrant les requêtes LINQ avancées via les services
/// </summary>
public class ReportsController : Controller
{
    private readonly ICustomerService _customerService;
    private readonly IMovieService _movieService;

    public ReportsController(ICustomerService customerService, IMovieService movieService)
    {
        _customerService = customerService;
        _movieService = movieService;
    }

    /// <summary>
    /// LINQ Query: Get customers with DiscountRate > 20
    /// </summary>
    public async Task<IActionResult> HighDiscountCustomers()
    {
        var customers = await _customerService.GetCustomersWithHighDiscountAsync(20);
        ViewBag.Title = "Clients avec remise > 20%";
        return View("CustomerList", customers);
    }

    /// <summary>
    /// LINQ Query: Order movies by DateAjoutMovie descending
    /// </summary>
    public async Task<IActionResult> RecentMovies()
    {
        var movies = await _movieService.GetMoviesOrderedByDateAsync();
        ViewBag.Title = "Films récents (triés par date)";
        return View("MovieList", movies);
    }

    /// <summary>
    /// LINQ Query: Group movies by Genre
    /// </summary>
    public async Task<IActionResult> MoviesByGenre()
    {
        var groupedMovies = await _movieService.GetMoviesGroupedByGenreAsync();
        return Json(groupedMovies);
    }

    /// <summary>
    /// LINQ Query: Count number of customers per MembershipType
    /// </summary>
    public async Task<IActionResult> CustomerStatistics()
    {
        var stats = await _customerService.GetCustomerStatsByMembershipTypeAsync();
        ViewBag.Title = "Statistiques clients par type d'adhésion";
        return Json(stats);
    }

    /// <summary>
    /// Get all customers with MembershipType included (eager loading)
    /// </summary>
    public async Task<IActionResult> CustomersWithMembership()
    {
        var customers = await _customerService.GetAllCustomersWithMembershipAsync();
        ViewBag.Title = "Clients avec type d'adhésion";
        return View("CustomerList", customers);
    }

    /// <summary>
    /// Get all movies with Genre included (eager loading)
    /// </summary>
    public async Task<IActionResult> MoviesWithGenre()
    {
        var movies = await _movieService.GetAllMoviesWithGenreAsync();
        ViewBag.Title = "Films avec genre";
        return View("MovieList", movies);
    }
}
