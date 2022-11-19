using Microsoft.AspNetCore.Identity;

namespace AirsoftMatchMaker.Infrastructure.Data.Entities
{
    public class User : IdentityUser
    {
        public decimal Credits { get; set; } = 200;
        public virtual ICollection<Bet> Bets { get; set; } = new HashSet<Bet>();
    }
}
