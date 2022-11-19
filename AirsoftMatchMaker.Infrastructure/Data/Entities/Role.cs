using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AirsoftMatchMaker.Infrastructure.Data.Entities
{
    public class Role : IdentityRole
    {
        [Required]
        [MaxLength(200)]
        public string Description { get; set; } = null!;
    }
}
