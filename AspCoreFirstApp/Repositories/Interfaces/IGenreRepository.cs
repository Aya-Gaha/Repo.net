using AspCoreFirstApp.Models;

namespace AspCoreFirstApp.Repositories.Interfaces;

public interface IGenreRepository
{
    Task<IEnumerable<Genre>> GetAllAsync();
    Task<Genre?> GetByIdAsync(Guid id);
}
