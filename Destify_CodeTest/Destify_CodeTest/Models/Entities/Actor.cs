using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace Destify_CodeTest.Models.Entities
{
    /// <summary>
    /// Actor Entity
    /// </summary>
    public class Actor
    {
        public Actor()
        {
            this.Movies = new HashSet<Movie>();
        }

        /// <summary>
        /// The Id of the actor.  Set by database.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }

        /// <summary>
        /// The Actor's name
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Movie Entities the actor was in
        /// </summary>
        public virtual ICollection<Movie> Movies { get; set; }
    }
}
