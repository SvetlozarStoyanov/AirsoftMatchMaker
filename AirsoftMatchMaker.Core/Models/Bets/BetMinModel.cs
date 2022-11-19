namespace AirsoftMatchMaker.Core.Models.Bets
{
    public class BetMinModel
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public int WinningTeamId { get; set; }
        public string WinningTeamName { get; set; } = null!;
        public decimal CreditsBet { get; set; }

    }
}
