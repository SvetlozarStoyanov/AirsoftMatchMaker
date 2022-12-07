namespace AirsoftMatchMaker.Core.Models.GameModes
{
    public class GameModeListModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int MapsCount { get; set; }
        public int PointsToWin { get; set; }
    }
}
