using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace Destify_CodeTest.Models.Entities
{
    /// <summary>
    /// Movie Rating entity
    /// </summary>
    public class MovieRating
    {
        /// <summary>
        /// The Id of the rating.  Set by database
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }

        /// <summary>
        /// The Rating of the movie
        /// </summary>
        [Required]
        public int Rating { get; set; }

        /// <summary>
        /// The Id of the movie entity this rating belongs to.
        /// </summary>
        public int MovieId { get; set; }

        /// <summary>
        /// The movie entity this rating belongs to.
        /// </summary>
        [ForeignKey("MovieId")]
        public Movie Movie { get; set; }
    }
}
