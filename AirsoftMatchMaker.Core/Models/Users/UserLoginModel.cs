using System.ComponentModel.DataAnnotations;

namespace AirsoftMatchMaker.Core.Models.Users
{
    public class UserLoginModel
    {
        public string Username { get; set; } = null!;
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
