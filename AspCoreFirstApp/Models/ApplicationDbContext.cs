namespace AspCoreFirstApp.Models;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;


public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options):base(options) {}
    public DbSet<Movie>? Movies { get; set; } 
    public DbSet<Genre> Genres { get; set; }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<MembershipType> MembershipTypes { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MembershipType>()
            .Property(mt => mt.SignUpFee)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Genre>()
            .Property(g => g.Id)
            .HasDefaultValueSql("NEWID()");
        modelBuilder.Entity<Movie>()
            .HasOne(m => m.Genre)
            .WithMany(g => g.Movies)
            .HasForeignKey(m => m.GenreId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Customer>()
            .HasOne(c => c.MembershipType)
            .WithMany(mt => mt.Customers)
            .HasForeignKey(c => c.MembershipTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed MembershipTypes
        modelBuilder.Entity<MembershipType>().HasData(
            new MembershipType { Id = 1, SignUpFee = 0m, DurationInMonth = 1, DiscountRate = 0 },
            new MembershipType { Id = 2, SignUpFee = 30m, DurationInMonth = 6, DiscountRate = 10 },
            new MembershipType { Id = 3, SignUpFee = 90m, DurationInMonth = 12, DiscountRate = 20 }
        );

        // Seed Customers (using FK only, no navigation properties)
        modelBuilder.Entity<Customer>().HasData(
            new Customer { Id = 1, Name = "Aya", MembershipTypeId = 1 },
            new Customer { Id = 2, Name = "Hend", MembershipTypeId = 2 },
            new Customer { Id = 3, Name = "Sami", MembershipTypeId = 3 },
            new Customer { Id = 4, Name = "Karim", MembershipTypeId = 2 }
        );

        var actionId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var scifiId  = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var dramaId  = Guid.Parse("33333333-3333-3333-3333-333333333333");
        modelBuilder.Entity<Genre>().HasData(
            new Genre { Id = actionId, Name = "Action" },
            new Genre { Id = scifiId, Name = "Sci-Fi" },
            new Genre { Id = dramaId, Name = "Drama" });

        // Seed Movies from Movies.json if present; fallback to existing hardcoded data.
        var seededFromJson = TrySeedMoviesFromJson(modelBuilder);
        if (!seededFromJson)
        {
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

    private static bool TrySeedMoviesFromJson(ModelBuilder modelBuilder)
    {
        try
        {
            var baseDir = AppContext.BaseDirectory;

            // The csproj copies Data/Movies.json to output.
            var path = Path.Combine(baseDir, "Data", "Movies.json");
            if (!File.Exists(path))
            {
                // optional fallback
                var alt = Path.Combine(baseDir, "Movies.json");
                if (!File.Exists(alt))
                    return false;
                path = alt;
            }

            var json = File.ReadAllText(path);
            var movies = JsonSerializer.Deserialize<List<Movie>>(json, new JsonSerializerOptions(JsonSerializerDefaults.Web));
            if (movies is null || movies.Count == 0)
                return false;

            modelBuilder.Entity<Movie>().HasData(movies);
            return true;
        }
        catch
        {
            return false;
        }
    }
}