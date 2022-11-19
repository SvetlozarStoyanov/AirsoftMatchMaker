namespace AirsoftMatchMaker.Core.Models.Teams
{
    public class TeamListModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int AverageSkillPoints { get; set; }
        public int PlayersCount { get; set; }
    }
}
