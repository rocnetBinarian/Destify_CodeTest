using Microsoft.EntityFrameworkCore;

namespace Destify_CodeTest.Models.Entities
{
    public class MovieContext : DbContext
    {
        public static string CONNSTRING { get; set; }
        public MovieContext()
        {

        }

        public MovieContext(DbContextOptions<MovieContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(CONNSTRING);
            }
        }


        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<MovieRating> MovieRatings { get; set; }
    }
}
