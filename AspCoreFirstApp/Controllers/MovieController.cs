using Microsoft.AspNetCore.Mvc;
using AspCoreFirstApp.Models;
using AspCoreFirstApp.Models.ViewModels;
using X.PagedList;
using X.PagedList.Extensions;

namespace AspCoreFirstApp.Controllers;
using Microsoft.EntityFrameworkCore;   // at the top of the file

public class MovieController : Controller
{
    private readonly ApplicationDbContext _dbContext;

    public MovieController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    // TP1
    // GET
    // public IActionResult Index()
    // { 
    //     /*var movies = new List<Movie>()
    //     {
    //         new Movie {Id=1 , Name="Inception"}, 
    //         new Movie {Id=2 , Name="The Matrix"}, 
    //         new Movie {Id=3 , Name="Interstellar"},
    //         new Movie {Id=4 , Name="Blade Runner 2049"},
    //     }; */
    //     var movies = _dbContext.Movies.ToList();
    //     return View(movies);
    // }
    // public IActionResult Index(string sortOrder)
    // {
    //     ViewData["NameSort"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
    //
    //     var movies = from m in _dbContext.Movies
    //         select m;
    //
    //     if (sortOrder == "name_desc")
    //         movies = movies.OrderByDescending(m => m.Name);
    //     else
    //         movies = movies.OrderBy(m => m.Name);
    //
    //     return View(movies.ToList());
    // }
    
    public IActionResult Index(string sortOrder, int? page)
    {
        // sortOrder = tri courant
        ViewData["CurrentSort"] = sortOrder;

        // NameSort = tri à appliquer au prochain clic sur l'en-tête "Name"
        ViewData["NameSort"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

        var movies = _dbContext.Movies.AsQueryable()
            .Include(m => m.Genre)
            .AsQueryable();

        movies = sortOrder == "name_desc"
            ? movies.OrderByDescending(m => m.Name)
            : movies.OrderBy(m => m.Name);

        int pageSize = 5;
        int pageNumber = page ?? 1;

        return View(movies.ToPagedList(pageNumber, pageSize));
    }

    public IActionResult detailsList(int id)
    {
        var movie = _dbContext.Movies.FirstOrDefault(m => m.Id == id);
        if (movie == null)
            return NotFound();
        return View(movie);
    }
    public IActionResult Edit(int id)
    {   
        var movie = _dbContext.Movies.Find(id);
        if (movie == null)
            return NotFound();
        return View(movie);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Movie movie)
    {
        Console.WriteLine("POST ID = " + movie.Id);
        if (!ModelState.IsValid)
            return View(movie);

        var movieInDb = _dbContext.Movies.FirstOrDefault(m => m.Id == movie.Id);
        if (movieInDb == null)
            return NotFound();

        // Mettre à jour uniquement les champs édités par le formulaire
        movieInDb.Name = movie.Name;

        _dbContext.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var movie = _dbContext.Movies.Find(id);
        if (movie == null)
            return NotFound();

        return View(movie);
    }
    
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var movie = _dbContext.Movies.Find(id);
        _dbContext.Movies.Remove(movie);
        _dbContext.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
    

    //TP 1
    // public IActionResult Edit(int id)
    // {
    //     return Content("Test Id "+id);
    // }

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
    

    // TP1 details movie
    // public IActionResult DetailsList(int id)
    // {
    //     var movie = new Movie
    //     {
    //         Id = id,
    //         Name = id switch
    //         {
    //             1 => "Inception",
    //             2 => "The Matrix",
    //             3 => "Interstellar",
    //             4 => "Blade Runner 2049",
    //             _ => "Unknown"
    //         }
    //     };
    //     return View(movie);
    // }

}
