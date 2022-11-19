using AirsoftMatchMaker.Core.Models.Games;
using AirsoftMatchMaker.Core.Models.Maps;

namespace AirsoftMatchMaker.Core.Models.GameModes
{
    public class GameModeViewModel
    {
        public GameModeViewModel()
        {
            Maps = new HashSet<MapMinModel>();
            Games = new HashSet<GameMinModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int PointsToWin { get; set; }
        public virtual ICollection<MapMinModel> Maps { get; set; }
        public virtual ICollection<GameMinModel> Games { get; set; }
    }
}
