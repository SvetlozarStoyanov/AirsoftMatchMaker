namespace AirsoftMatchMaker.Core.Models.Maps
{
    public class MapSelectModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int GameModeId { get; set; }
        public string GameModeName { get; set; } = null!;
    }
}
