using Destify_CodeTest.Models.ViewModels;

namespace Destify_CodeTest.Models.Services
{
    /// <summary>
    /// Actor service interface
    /// </summary>
    public interface IActorService
    {
        int Create(Entities.Actor actor);
        List<Entities.Actor> GetAll();
        Entities.Actor GetById(int id);
        List<Entities.Actor> GetByMovieId(int id);
        List<Entities.Actor> Search(string query);
        Entities.Actor Update(int actorId, Entities.Actor actor);
        Exception Replace(int actorId, Entities.Actor actor);
        bool DeleteById(int id);
        s_Actor BuildActorVM(Entities.Actor actor);
    }
}
