using System.ComponentModel.DataAnnotations;

namespace AirsoftMatchMaker.Core.Models.Teams
{
    public class TeamCreateModel
    {
        [Required]
        [MinLength(3), MaxLength(35)]
        public string Name { get; set; } = null!;
    }
}
