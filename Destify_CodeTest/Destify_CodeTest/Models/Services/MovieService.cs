﻿using Destify_CodeTest.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Destify_CodeTest.Models.Services
{
    public class MovieService : IMovieService
    {
        private readonly MovieContext _context;
        public MovieService(MovieContext context)
        {
            _context = context;
        }
        public int Create(Movie movie)
        {
            _context.Movies.Add(movie);
            _context.SaveChanges();
            return movie.Id;
        }

        public bool DeleteById(int id)
        {
            var movie = _context.Movies.FirstOrDefault(x => x.Id == id);
            if (movie == default)
            {
                return false;
            }
            _context.Movies.Remove(movie);
            _context.SaveChanges();
            return true;
        }

        public List<Movie> GetAll()
        {
            return _context.Movies.ToList();
        }

        public List<Movie> GetByActorId(int id)
        {
            var actor = _context.Actors.FirstOrDefault(x => x.Id == id);
            if (actor == default)
                return null;
            return _context.Movies
                .Where(x => x.Actors.Contains(actor))
                .ToList();
        }

        public Movie GetById(int id)
        {
            return _context.Movies.FirstOrDefault(x => x.Id == id);
        }

        public List<Movie> Search(string query)
        {
            return _context.Movies
                .Where(x => x.Name.Contains(query))
                .ToList();
        }

        public Movie Update(Movie movie)
        {
            var dbMovie = GetById(movie.Id);
            if (dbMovie == default)
                return null;
            dbMovie.Name = movie.Name ?? dbMovie.Name;
            var newRatingIds = movie.MovieRatings.Select(r => r.Id).Except(dbMovie.MovieRatings.Select(r => r.Id));
            var newRatings = movie.MovieRatings.Where(r => newRatingIds.Contains(r.Id)).ToList();

            newRatings.ForEach(r => {
                dbMovie.MovieRatings.Add(r);
            });
            _context.SaveChanges();
            return dbMovie;
        }

        public Exception Replace(int movieId, Movie movie)
        {
            var movieById = GetById(movieId);
            if (movieById == default) {
                return new KeyNotFoundException("Could not find movie with id " + movieId);
            }
            if (movie.Id != default(int) && movie.Id != movieId) {
                return new ArgumentException("MovieId in request path must match MovieId in request body");
            }
            try {
                _context.Entry(movieById).CurrentValues.SetValues(movie);
                _context.SaveChanges();
            } catch (Exception ex) {
                return ex;
            }
            return null;
        }
    }
}
