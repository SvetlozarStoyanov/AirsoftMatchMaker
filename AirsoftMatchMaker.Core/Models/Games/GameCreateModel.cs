using AirsoftMatchMaker.Core.Models.Maps;
using AirsoftMatchMaker.Core.Models.Teams;
using System.ComponentModel.DataAnnotations;

namespace AirsoftMatchMaker.Core.Models.Games
{
    public class GameCreateModel
    {
        //[Required]
        [MinLength(5), MaxLength(60)]
        public string? Name { get; set; }
        public string DateString { get; set; }
        [Range(20.00, 100)]
        public decimal EntryFee { get; set; }
        //public int GameModeId { get; set; }
        //public ICollection<GameModeSelectModel> GameModes { get; set; } = null!;
        public int MapId { get; set; }
        public ICollection<MapSelectModel> Maps { get; set; } = null!;
        public int TeamRedId { get; set; }
        [NotEqual(nameof(TeamRedId), ErrorMessage = "Teams cannot be the same team")]
        public int TeamBlueId { get; set; }
        public ICollection<TeamSelectModel> Teams { get; set; } = null!;
    }
}
