namespace AspCoreFirstApp.Models;

public class Genre
{
    public Guid Id {get; set;}  // Guid= Globally unique identifier
    public string Name { get; set; }
    
    //relation (1->many)
    public ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
