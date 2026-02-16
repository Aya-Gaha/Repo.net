using Microsoft.AspNetCore.Http;

namespace AspCoreFirstApp.Models.ViewModels;

public class MovieVM
{
    public Movie Movie { get; set; } = new();

    public IFormFile? Photo { get; set; }
}
