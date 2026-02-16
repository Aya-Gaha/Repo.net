namespace AspCoreFirstApp.Models;
using Microsoft.EntityFrameworkCore;


public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options):base(options) {}
    public DbSet<Movie>? Movies { get; set; } 
    public DbSet<Genre> Genres { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Genre>()
            .Property(g => g.Id)
            .HasDefaultValueSql("NEWID()");
        modelBuilder.Entity<Movie>()
            .HasOne(m => m.Genre)
            .WithMany(g => g.Movies)
            .HasForeignKey(m => m.GenreId)
            .OnDelete(DeleteBehavior.Restrict);
        var actionId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var scifiId  = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var dramaId  = Guid.Parse("33333333-3333-3333-3333-333333333333");
        modelBuilder.Entity<Genre>().HasData(
            new Genre { Id = actionId, Name = "Action" },
            new Genre { Id = scifiId, Name = "Sci-Fi" },
            new Genre { Id = dramaId, Name = "Drama" });
        
        modelBuilder.Entity<Movie>().HasData(
            new Movie
            {
                Id = 1,
                Name = "Inception",
                GenreId = scifiId
            },
            new Movie
            {
                Id = 2,
                Name = "Interstellar",
                GenreId = scifiId
            },
            new Movie
            {
                Id = 3,
                Name = "The Matrix",
                GenreId = scifiId
            }
        );
        
    }
}