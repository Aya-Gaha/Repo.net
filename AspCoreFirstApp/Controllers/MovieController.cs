using Microsoft.AspNetCore.Mvc;
using AspCoreFirstApp.Models;
using AspCoreFirstApp.Models.ViewModels;
namespace AspCoreFirstApp.Controllers;

public class MovieController : Controller
{
    // GET
    public IActionResult Index()
    { 
        Movie movie = new Movie 
        { 
            Id = 1 , Name = "Movie 1"
        };
        List<Movie> movies = new List<Movie>()
        {
            new Movie {Id=2 , Name="Movie 2"}, 
            new Movie {Id=3 , Name="Movie 3"}, 
            new Movie {Id=4 , Name="Movie 4"},
        }; 
        return View(movies);
    }
    
    public IActionResult Edit(int id)
    {
        return Content("Test Id "+id);
    }

    public IActionResult ByRelease(int month, int year)
    {
        return Content("Test Month " + month + " " + year);
    }
    
    //ViewModel PART
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
}