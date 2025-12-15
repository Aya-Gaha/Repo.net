using Microsoft.AspNetCore.Mvc;
using AspCoreFirstApp.Models;
namespace AspCoreFirstApp.Controllers;

[Route("Movie")]
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

    [HttpGet("Released/{year:int}/{month:int}")]
    public IActionResult ByRelease(int year, int month)
    {
        return Content($"[Attribute Routing] {month}/{year}");
    }
}