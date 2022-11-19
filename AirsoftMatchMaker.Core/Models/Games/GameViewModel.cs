using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirsoftMatchMaker.Core.Models.Bets;
using AirsoftMatchMaker.Core.Models.Teams;
using AirsoftMatchMaker.Core.Models.Players;

namespace AirsoftMatchMaker.Core.Models.Games
{
    public class GameViewModel
    {

        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string Date { get; set; }
        public decimal EntryFee { get; set; }
        public GameStatus GameStatus { get; set; }
        public int MapId { get; set; }
        public string MapName { get; set; } = null!;
        public int GameModeId { get; set; }
        public string GameModeName { get; set; } = null!;
        public int MatchmakerId { get; set; }
        public string MatchmakerName { get; set; }
        public string? Result { get; set; }

        public int TeamRedId { get; set; }
        public string TeamRedName { get; set; } = null!;
        public ICollection<PlayerMinModel> TeamRedPlayers { get; set; }
        public int TeamRedPoints { get; set; }
        public int BetsForTeamRed { get; set; }

        public int TeamBlueId { get; set; }
        public string TeamBlueName { get; set; } = null!;
        public ICollection<PlayerMinModel> TeamBluePlayers { get; set; }
        public int TeamBluePoints { get; set; }
        public int BetsForTeamBlue { get; set; }

    }
}
