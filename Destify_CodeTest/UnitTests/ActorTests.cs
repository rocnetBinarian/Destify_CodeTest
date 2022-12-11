using Destify_CodeTest.Controllers;
using Destify_CodeTest.Models.Entities;
using Destify_CodeTest.Models.Services;

namespace UnitTests
{
    public class ActorTests
    {
        private readonly ActorController actorController;
        private readonly IActorService actorService;
        public ActorTests()
        {
            MovieContext.CONNSTRING = "Data Source=movies.test.db";
            var dbContext = new MovieContext();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            dbContext.SaveChanges();
            actorService = new ActorService(dbContext);
            actorController = new ActorController(actorService);

        }

        [Fact]
        public void SanityCheck()
        {
            Assert.Empty(actorService.GetAll());
        }

        [Fact]
        public void TestServiceCreate()
        {
            int newId = actorService.Create(new Actor()
            {
                Id = 20,
                Name = "Bobby Bobbington",
                Movies = new List<Movie>()
                {
                    new Movie()
                    {
                        Id = 17,
                        Name = "My First Movie"
                    },
                    new Movie()
                    {
                        Id = 22,
                        Name = "My Second Movie",
                        MovieRatings = new List<MovieRating>()
                        {
                            new MovieRating()
                            {
                                Id = 30,
                                Rating = 5
                            }
                        }
                    }
                }
            });

            Assert.Equal(1, newId);
        }
    }
}