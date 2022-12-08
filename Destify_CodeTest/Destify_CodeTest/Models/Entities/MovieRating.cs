using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Destify_CodeTest.Models.Entities
{
    public class MovieRating
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Rating { get; set; }

        public int MovieId { get; set; }
        [ForeignKey("MovieId")]
        public Movie Movie { get; set; }
    }
}
