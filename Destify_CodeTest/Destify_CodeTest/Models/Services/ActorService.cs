using Destify_CodeTest.Models.Entities;

namespace Destify_CodeTest.Models.Services
{
    public class ActorService : IActorService
    {
        private readonly MovieContext _context;

        public ActorService(MovieContext context)
        {
            _context = context;
        }

        public bool Create(Actor actor)
        {

            try
            {
                _context.Actors.Add(actor);
                _context.SaveChanges();
            } catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool DeleteById (int id)
        {
            var actor = _context.Actors.FirstOrDefault(x => x.Id == id);
            if (actor == default)
            {
                return false;
            }
            _context.Actors.Remove(actor);
            _context.SaveChanges();
            return true;
        }

        public List<Actor> GetAll()
        {
            return _context.Actors.ToList();
        }

        public Actor GetById(int id)
        {
            return _context.Actors.FirstOrDefault(x => x.Id == id);
        }

        public List<Actor> Search(string query)
        {
            return _context.Actors
                .Where(x => x.Name.Contains(query))
                .ToList();
        }

        public Actor Update(Actor actor)
        {
            throw new NotImplementedException();
        }
    }
}
