namespace AirsoftMatchMaker.Core.Models.Teams
{
    public class TeamSelectModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public double AverageSkillPoints { get; set; }
    }
}
