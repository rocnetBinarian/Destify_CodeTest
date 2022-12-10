namespace Destify_CodeTest.Models.Services
{
    public interface IActorService
    {
        int Create(Entities.Actor actor);
        List<Entities.Actor> GetAll();
        Entities.Actor GetById(int id);
        List<Entities.Actor> GetByMovieId(int id);
        List<Entities.Actor> Search(string query);
        Entities.Actor Update(Entities.Actor actor);
        Exception Replace(int actorId, Entities.Actor actor);
        bool DeleteById(int id);
    }
}
