﻿namespace Destify_CodeTest.Models.Services
{
    public interface IMovieRatingService
    {
        bool Create(Entities.MovieRating rating);
        List<Entities.MovieRating> GetAll();
        Entities.MovieRating GetById(int id);
        Entities.MovieRating Update(Entities.MovieRating rating);
        bool DeleteById(int id);
    }
}
