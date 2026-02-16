namespace AspCoreFirstApp.Models;

public class Movie
{
    public int Id { get; set; }
    public string? Name { get; set; } = string.Empty;

    //Foreign Key
    public Guid GenreId { get; set; }

    // Navigation property: nullable, car elle n'est pas envoyée par les formulaires
    public Genre? Genre { get; set; } = null!;

    public string? ImageFile { get; set; }

    public DateTime? DateAjoutMovie { get; set; }

    public Movie()
    {
    }
}