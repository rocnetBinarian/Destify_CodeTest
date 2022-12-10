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

        public int Create(Actor actor)
        {
            _context.Actors.Add(actor);
            _context.SaveChanges();
            return actor.Id;
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

        public List<Actor> GetByMovieId(int id)
        {
            var movie = _context.Movies.FirstOrDefault(x => x.Id == id);
            if (movie == default)
                return null;
            return _context.Actors
                .Where(x => x.Movies.Contains(movie))
                .ToList();
        }

        public List<Actor> Search(string query)
        {
            return _context.Actors
                .Where(x => x.Name.Contains(query))
                .ToList();
        }

        public Actor Update(Actor actor)
        {
            var dbActor = _context.Actors.FirstOrDefault(x => x.Id == actor.Id);
            if (dbActor == default)
                return null;
            dbActor.Name = actor.Name ?? dbActor.Name;
            var newMovieIds = actor.Movies.Select(m => m.Id).Except(dbActor.Movies.Select(m => m.Id));
            var newMovies = actor.Movies.Where(m => newMovieIds.Contains(m.Id)).ToList();
            
            newMovies.ForEach(m => {
                dbActor.Movies.Add(m);
            });
            _context.SaveChanges();
            return dbActor;
        }

        public Exception Replace(int actorId, Actor actor)
        {
            var actorById = GetById(actorId);
            if (actorById == default) {
                return new KeyNotFoundException("Could not find actor with id "+actorId);
            }
            if (actor.Id != actorId) {
                return new ArgumentException("ActorId in request path must match ActorId in request body");
            }
            try {
                _context.Entry(actorById).CurrentValues.SetValues(actor);
                _context.SaveChanges();
            } catch (Exception ex) {
                return ex;
            }
            return null;
        }
    }
}
