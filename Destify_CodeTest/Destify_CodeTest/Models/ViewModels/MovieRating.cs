using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations;

namespace Destify_CodeTest.Models.ViewModels
{
    public struct s_MovieRating
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public s_MovieRatingMovie Movie { get; set; }
    }

    public struct s_MovieRatingMovie
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
