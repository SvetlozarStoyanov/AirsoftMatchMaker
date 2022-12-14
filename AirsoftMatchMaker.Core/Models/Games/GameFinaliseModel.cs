using System.ComponentModel.DataAnnotations;

namespace AirsoftMatchMaker.Core.Models.Games
{
    public class GameFinaliseModel
    {
        public int Id { get; set; }
        public int GameModeMaxPoints { get; set; }
        [Required(ErrorMessage = "Required!")]
        [Range(0, 100, ErrorMessage = "Points must be within the given limit")]
        public int TeamRedPoints { get; set; }
        [Required(ErrorMessage = "Required")]
        [Range(0, 100, ErrorMessage = "Points must be within the given limit")]
        [NotEqual(nameof(TeamRedPoints), ErrorMessage = "Points cannot be equal!")]
        public int TeamBluePoints { get; set; }
    }
}
