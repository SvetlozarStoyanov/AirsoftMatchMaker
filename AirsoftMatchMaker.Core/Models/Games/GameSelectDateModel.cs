namespace AirsoftMatchMaker.Core.Models.Games
{
    public class GameSelectDateModel
    {
        public GameSelectDateModel()
        {
            Dates = new HashSet<string>();
        }
        public string DateTime { get; set; }
        public ICollection<string> Dates { get; set; }
    }
}
