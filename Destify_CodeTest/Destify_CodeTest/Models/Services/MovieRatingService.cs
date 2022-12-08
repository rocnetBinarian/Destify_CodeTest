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

        public MovieRating Update(MovieRating rating)
        {
            throw new NotImplementedException();
        }
    }
}
