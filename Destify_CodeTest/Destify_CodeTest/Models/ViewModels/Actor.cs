namespace Destify_CodeTest.Models.ViewModels
{
    public struct s_Actor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<s_MoviesContainingActor> Movies { get; set; }
    }

    public struct s_MoviesContainingActor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? AvgRating { get; set; }
    }
}