﻿using System.Runtime.InteropServices;

namespace Destify_CodeTest.Models.Services
{
    public interface IMovieService
    {
        bool Create(Entities.Movie movie);
        List<Entities.Movie> GetAll();
        Entities.Movie GetById(int id);
        List<Entities.Movie> Search(string query);
        Entities.Movie Update(Entities.Movie movie);
        bool DeleteById(int id);
    }
}
