namespace Destify_CodeTest.Models.ViewModels
{
    /// <summary>
    /// Struct to hold non-circular-reference entity data
    /// </summary>
    public struct s_Actor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<s_MoviesContainingActor> Movies { get; set; }
    }

    /// <summary>
    /// Struct to hold non-circular-reference entity data
    /// </summary>
    public struct s_MoviesContainingActor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? AvgRating { get; set; }
    }
}