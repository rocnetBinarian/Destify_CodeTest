using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace Destify_CodeTest.Models.Entities
{
    [Table("Movies")]
    public class Movie
    {
        public Movie()
        {
            this.Actors = new HashSet<Actor>();
            this.MovieRatings = new List<MovieRating>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual ICollection<MovieRating> MovieRatings { get; set; }

        public virtual ICollection<Actor> Actors { get; set; }
    }
}
