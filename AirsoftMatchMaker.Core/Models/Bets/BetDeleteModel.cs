namespace AirsoftMatchMaker.Core.Models.Bets
{
    public class BetDeleteModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int GameId { get; set; }
        public string GameName { get; set; }
        public int? WinningTeamId { get; set; }
        public string? WinningTeamName { get; set; }
        public decimal? PotentialProfit { get; set; }
        public decimal? CreditsBet { get; set; }
        public int? Odds { get; set; }
    }
}
