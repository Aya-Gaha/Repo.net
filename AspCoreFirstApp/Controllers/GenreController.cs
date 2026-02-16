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

    [HttpGet]
    public IActionResult Edit(Guid id)
    {
        var genre = _dbContext.Genres.Find(id);
        if (genre == null)
            return NotFound();

        return View(genre);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Genre genre)
    {
        if (!ModelState.IsValid)
            return View(genre);

        var genreInDb = _dbContext.Genres.FirstOrDefault(g => g.Id == genre.Id);
        if (genreInDb == null)
            return NotFound();

        genreInDb.Name = genre.Name;

        _dbContext.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Delete(Guid id)
    {
        var genre = _dbContext.Genres.Find(id);
        if (genre == null)
            return NotFound();

        return View(genre);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(Guid id)
    {
        var genre = _dbContext.Genres.Find(id);
        if (genre == null)
            return NotFound();

        _dbContext.Genres.Remove(genre);
        _dbContext.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}