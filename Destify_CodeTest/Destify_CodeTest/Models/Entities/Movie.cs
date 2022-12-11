using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace Destify_CodeTest.Models.Entities
{
    /// <summary>
    /// Movie Entity
    /// </summary>
    public class Movie
    {
        public Movie()
        {
            this.Actors = new HashSet<Actor>();
            this.MovieRatings = new List<MovieRating>();
        }

        /// <summary>
        /// The Id of the movie.  Set by database
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }

        /// <summary>
        /// The Movie's name
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Collection of ratings for the movie
        /// </summary>
        public virtual ICollection<MovieRating> MovieRatings { get; set; }

        /// <summary>
        /// Collection of actors in the movie
        /// </summary>
        public virtual ICollection<Actor> Actors { get; set; }
    }
}
