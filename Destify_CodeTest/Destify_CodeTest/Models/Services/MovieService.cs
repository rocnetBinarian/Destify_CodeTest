using Destify_CodeTest.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Destify_CodeTest.Models.Services
{
    public class MovieService : IMovieService
    {
        private readonly MovieContext _context;
        public MovieService(MovieContext context)
        {
            _context = context;
        }
        public int Create(Movie movie)
        {
            _context.Movies.Add(movie);
            _context.SaveChanges();
            return movie.Id;
        }

        public bool DeleteById(int id)
        {
            var movie = _context.Movies.FirstOrDefault(x => x.Id == id);
            if (movie == default)
            {
                return false;
            }
            _context.Movies.Remove(movie);
            _context.SaveChanges();
            return true;
        }

        public List<Movie> GetAll()
        {
            return _context.Movies.ToList();
        }

        public Movie GetById(int id)
        {
            return _context.Movies.FirstOrDefault(x => x.Id == id);
        }

        public List<Movie> Search(string query)
        {
            return _context.Movies
                .Where(x => x.Name.Contains(query))
                .ToList();
        }

        public Movie Update(Movie movie)
        {
            throw new NotImplementedException();
        }
    }
}
