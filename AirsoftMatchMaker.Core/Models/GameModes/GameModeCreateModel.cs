using System.ComponentModel.DataAnnotations;

namespace AirsoftMatchMaker.Core.Models.GameModes
{
    public class GameModeCreateModel
    {
        [Required]
        [MinLength(5), MaxLength(30)]
        public string Name { get; set; } = null!;
        [Required]
        [MinLength(5), MaxLength(200)]
        public string Description { get; set; } = null!;
        [Range(1, 100)]
        public int PointsToWin { get; set; }

    }
}
