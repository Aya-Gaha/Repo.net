using AspCoreFirstApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspCoreFirstApp.Controllers;

public class GenreController : Controller
{
    private readonly ApplicationDbContext _dbContext;

    public GenreController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IActionResult Index()
    {
        var genres = _dbContext.Genres.ToList();
        return View(genres);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Genre genre)
    {
        if (ModelState.IsValid)
        {
            _dbContext.Genres.Add(genre);
            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        return View(genre);
    }
}