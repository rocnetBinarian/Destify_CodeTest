using Destify_CodeTest.Models.Entities;
using Destify_CodeTest.Models.ViewModels;
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
            return _context.Movies
                .Include(x => x.Actors)
                .Include(x => x.MovieRatings)
                .ToList();
        }

        public List<Movie> GetByActorId(int id)
        {
            var actor = _context.Actors.FirstOrDefault(x => x.Id == id);
            if (actor == default)
                return null;
            return _context.Movies
                .Where(x => x.Actors.Contains(actor))
                .Include(x => x.Actors)
                .Include(x => x.MovieRatings)
                .ToList();
        }

        public Movie GetById(int id)
        {
            return _context.Movies
                .Include(x => x.Actors)
                .Include(x => x.MovieRatings)
                .FirstOrDefault(x => x.Id == id);
        }

        public List<Movie> Search(string query)
        {
            return _context.Movies
                .Where(x => x.Name.Contains(query))
                .ToList();
        }

        public Movie Update(int movieId, Movie movie)
        {
            var dbMovie = GetById(movieId);
            if (dbMovie == default)
                return null;
            if (movie.Id != default(int) && movie.Id != movieId) {
                return dbMovie;
            }
            dbMovie.Name = movie.Name ?? dbMovie.Name;
            var newRatingIds = movie.MovieRatings.Select(r => r.Id).Except(dbMovie.MovieRatings.Select(r => r.Id));
            var newRatings = movie.MovieRatings.Where(r => newRatingIds.Contains(r.Id)).ToList();

            newRatings.ForEach(r => {
                dbMovie.MovieRatings.Add(r);
            });
            _context.SaveChanges();
            return dbMovie;
        }

        public Exception Replace(int movieId, Movie movie)
        {
            if (movie.Id != default(int) && movie.Id != movieId) {
                return new ArgumentException("MovieId in request path must match MovieId in request body");
            }
            var movieById = GetById(movieId);
            if (movieById == default) {
                return new KeyNotFoundException("Could not find movie with id " + movieId);
            }
            try {
                _context.Entry(movieById).CurrentValues.SetValues(movie);
                _context.SaveChanges();
            } catch (Exception ex) {
                return ex;
            }
            return null;
        }

        public s_Movie BuildMovieVM(Movie movie)
        {
            var actorList = new List<s_ActorsInMovie>();
            if (movie.Actors.Count > 0) {
                foreach (var actor in movie.Actors) {
                    actorList.Add(new s_ActorsInMovie() {
                        Id = actor.Id,
                        Name = actor.Name
                    });
                }
            }
            double? avgRating = null;
            if (movie.MovieRatings.Count > 0) {
                avgRating = movie.MovieRatings.Average(x => x.Rating);
            }
            var rtn = new s_Movie() {
                Id = movie.Id,
                Name = movie.Name,
                Actors = actorList,
                AvgRating = avgRating
            };
            return rtn;
        }
    }
}
