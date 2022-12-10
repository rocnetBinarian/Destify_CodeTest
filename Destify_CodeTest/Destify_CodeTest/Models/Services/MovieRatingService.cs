using Destify_CodeTest.Models.Entities;

namespace Destify_CodeTest.Models.Services
{
    public class MovieRatingService : IMovieRatingService
    {
        private readonly MovieContext _context;
        public MovieRatingService(MovieContext context)
        {
            _context = context;
        }

        public bool Create(MovieRating rating)
        {
            try
            {
                _context.MovieRatings.Add(rating);
                _context.SaveChanges();
            } catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool DeleteById(int id)
        {
            var movieRating = _context.MovieRatings.FirstOrDefault(r => r.Id == id);
            if (movieRating == default)
            {
                return false;
            }
            _context.MovieRatings.Remove(movieRating);
            _context.SaveChanges();
            return true;
        }

        public List<MovieRating> GetAll()
        {
            return _context.MovieRatings.ToList();
        }

        public MovieRating GetById(int id)
        {
            return _context.MovieRatings.FirstOrDefault(r => r.Id == id);
        }

        public List<MovieRating> GetByMovieId(int id)
        {
            return _context.MovieRatings
                .Where(x => x.MovieId == id)
                .ToList();
        }

        public MovieRating Update(MovieRating rating)
        {
            var dbRating = GetById(rating.Id);
            if (dbRating == default)
                return null;
            if (rating.Rating != default(int))
                dbRating.Rating = rating.Rating;
            // Other updates here, such as rater name, etc.
            _context.SaveChanges();
            return dbRating;
        }

        public Exception Replace(int MovieRatingId, MovieRating rating)
        {
            var ratingById = GetById(MovieRatingId);
            if (ratingById == default) {
                return new KeyNotFoundException("Could not find rating with id "+MovieRatingId);
            }
            if (MovieRatingId != default(int) && rating.Id != MovieRatingId) {
                return new ArgumentException("RatingId in request path must match RatingId in request body");
            }
            try {
                _context.Entry(ratingById).CurrentValues.SetValues(rating);
                _context.SaveChanges();
            } catch (Exception ex) {
                return ex;
            }
            return null;
        }
    }
}
