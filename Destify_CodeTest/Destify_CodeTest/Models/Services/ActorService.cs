using Destify_CodeTest.Models.Entities;
using Destify_CodeTest.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

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
            return _context.Actors
                .Include(x => x.Movies)
                .ThenInclude(x => x.MovieRatings)
                .ToList();
        }

        public Actor GetById(int id)
        {
            return _context.Actors
                .Include(x => x.Movies)
                .ThenInclude(x => x.MovieRatings)
                .FirstOrDefault(x => x.Id == id);
        }

        public List<Actor> GetByMovieId(int id)
        {
            var movie = _context.Movies.FirstOrDefault(x => x.Id == id);
            if (movie == default)
                return null;
            return _context.Actors
                .Where(x => x.Movies.Contains(movie))
                .Include(x => x.Movies)
                .ThenInclude(x => x.MovieRatings)
                .ToList();
        }

        public List<Actor> Search(string query)
        {
            return _context.Actors
                .Where(x => x.Name.Contains(query))
                .Include(x => x.Movies)
                .ThenInclude(x => x.MovieRatings)
                .ToList();
        }

        public Actor Update(int actorId, Actor actor)
        {
            var dbActor = GetById(actor.Id);
            if (dbActor == default)
                return null;
            if (actor.Id != default(int) && actor.Id != actorId) {
                return dbActor;
            }
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
            if (actor.Id != default(int) && actor.Id != actorId) {
                return new ArgumentException("ActorId in request path must match ActorId in request body");
            }
            var actorById = GetById(actorId);
            if (actorById == default) {
                return new KeyNotFoundException("Could not find actor with id "+actorId);
            }
            try {
                _context.Entry(actorById).CurrentValues.SetValues(actor);
                _context.SaveChanges();
            } catch (Exception ex) {
                return ex;
            }
            return null;
        }

        public s_Actor BuildActorVM(Actor actor)
        {
            var movieList = new List<s_MoviesContainingActor>();
            if (actor.Movies.Count > 0) {
                foreach (var movie in actor.Movies) {
                    double? avgRating = null;
                    if (movie.MovieRatings.Count > 0) {
                        avgRating = movie.MovieRatings.Average(x => x.Rating);
                    }
                    movieList.Add(new s_MoviesContainingActor() {
                        Id = movie.Id,
                        Name = movie.Name,
                        AvgRating = avgRating
                    });
                }
            }
            var rtn = new s_Actor() {
                Id = actor.Id,
                Name = actor.Name,
                Movies = movieList
            };
            return rtn;
        }
    }
}
