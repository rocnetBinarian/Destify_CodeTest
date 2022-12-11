using Destify_CodeTest.Models.ViewModels;

namespace Destify_CodeTest.Models.Services
{
    public interface IMovieRatingService
    {
        bool Create(Entities.MovieRating rating);
        List<Entities.MovieRating> GetAll();
        Entities.MovieRating GetById(int id);
        List<Entities.MovieRating> GetByMovieId(int id);
        Entities.MovieRating Update(int MovieRatingId, Entities.MovieRating rating);
        Exception Replace(int MovieRatingId, Entities.MovieRating rating);
        bool DeleteById(int id);
        s_MovieRating BuildRatingVM(Entities.MovieRating rating);
    }
}
