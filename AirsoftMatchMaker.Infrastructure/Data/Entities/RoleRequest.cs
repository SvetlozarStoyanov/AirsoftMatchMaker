using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirsoftMatchMaker.Infrastructure.Data.Entities
{
    public class RoleRequest
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(IdentityRole.Id))]
        public string RoleId { get; set; }
        public IdentityRole Role { get; set; }
        [ForeignKey(nameof(Entities.User.Id))]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
