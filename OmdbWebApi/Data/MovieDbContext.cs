using Microsoft.EntityFrameworkCore;
using OmdbWebApi.Entities;

namespace OmdbWebApi.Data
{
    public class MovieDbContext : DbContext
    {
        public MovieDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
    }
}
