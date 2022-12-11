using System.Runtime.InteropServices;
using Destify_CodeTest.Models.ViewModels;

namespace Destify_CodeTest.Models.Services
{
    public interface IMovieService
    {
        int Create(Entities.Movie movie);
        List<Entities.Movie> GetAll();
        Entities.Movie GetById(int id);
        List<Entities.Movie> GetByActorId(int id);
        List<Entities.Movie> Search(string query);
        Entities.Movie Update(int movieId, Entities.Movie movie);
        Exception Replace(int movieId, Entities.Movie movie);
        bool DeleteById(int id);
        s_Movie BuildMovieVM(Entities.Movie movie);
    }
}
