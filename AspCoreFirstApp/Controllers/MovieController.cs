using Microsoft.AspNetCore.Mvc;
using AspCoreFirstApp.Models;
using AspCoreFirstApp.Models.ViewModels;
namespace AspCoreFirstApp.Controllers;

public class MovieController : Controller
{
    // GET
    public IActionResult Index()
    { 
        var movies = new List<Movie>()
        {
            new Movie {Id=1 , Name="Inception"}, 
            new Movie {Id=2 , Name="The Matrix"}, 
            new Movie {Id=3 , Name="Interstellar"},
            new Movie {Id=4 , Name="Blade Runner 2049"},
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

    public IActionResult DetailsList(int id)
    {
        var movie = new Movie
        {
            Id = id,
            Name = id switch
            {
                1 => "Inception",
                2 => "The Matrix",
                3 => "Interstellar",
                4 => "Blade Runner 2049",
                _ => "Unknown"
            }
        };
        return View(movie);
    }
}
