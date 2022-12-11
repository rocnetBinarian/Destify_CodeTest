using Destify_CodeTest.Controllers;
using Destify_CodeTest.Models.Entities;
using Destify_CodeTest.Models.Services;
using Microsoft.AspNetCore.Mvc;

namespace UnitTests
{
    /// <summary>
    /// Provides only a small subset of tests as an example.
    /// </summary>
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

            Assert.Equal(20, newId);
            var movies = actorService.GetById(20).Movies.OrderByDescending(x => x.Id);
            Assert.Equal(22, movies.First().Id);
            Assert.Equal(17, movies.Skip(1).First().Id);
        }

        [Fact]
        public void TestServiceCreateNoId()
        {
            int newId = actorService.Create(new Actor()
            {
                Name = "Bobby Bobbington",
                Movies = new List<Movie>()
                {
                    new Movie()
                    {
                        Id = 0,
                        Name = "My First Movie"
                    },
                    new Movie()
                    {
                        Name = "My Second Movie",
                        MovieRatings = new List<MovieRating>()
                        {
                            new MovieRating()
                            {
                                Rating = 5
                            }
                        }
                    }
                }
            });

            Assert.Equal(1, newId);
            var movies = actorService.GetById(1).Movies.OrderByDescending(x => x.Id);
            Assert.Equal(2, movies.First().Id);
            Assert.Equal(1, movies.Skip(1).First().Id);
        }

        [Fact]
        public void TestCreate()
        {
            var newActor = new Actor()
            {
                Id = 33,
                Name = "Bobby Bobbington",
                Movies = new List<Movie>()
                {
                    new Movie()
                    {
                        Id = -5,
                        Name = "My First Movie"
                    },
                    new Movie()
                    {
                        Id = 55555,
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
            };
            var rtn = actorController.Create(newActor);
            Assert.True(rtn is CreatedResult);
            var cr = rtn as CreatedResult;
            var actor = cr.Value as Actor;
            Assert.Equal(33, actor.Id);
            var movies = actorService.GetById(actor.Id).Movies.OrderByDescending(x => x.Id);
            Assert.Equal(55555, movies.First().Id);
            Assert.Equal(-5, movies.Skip(1).First().Id);
        }
    }
}