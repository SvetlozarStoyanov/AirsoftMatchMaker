using System.ComponentModel.DataAnnotations;

namespace AirsoftMatchMaker.Core.Models.Users
{
    public class UserRegisterModel
    {
        [Required]
        [MinLength(3), MaxLength(40)]
        public string Username { get; set; } = null!;
        [Required]
        [MinLength(10), MaxLength(60)]
        public string Email { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        [MinLength(5), MaxLength(60)]
        public string Password { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
