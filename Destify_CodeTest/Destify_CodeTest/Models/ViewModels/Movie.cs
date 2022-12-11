
namespace Destify_CodeTest.Models.ViewModels {
    public struct s_Movie {
        public int Id {get; set;}
        public string Name {get; set;}
        public double? AvgRating {get; set;}
        public List<s_ActorsInMovie> Actors {get; set;}
    }

    public struct s_ActorsInMovie {
        public int Id {get; set;}
        public string Name {get; set;}
    }
}