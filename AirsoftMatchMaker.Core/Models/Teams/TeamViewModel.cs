﻿using AirsoftMatchMaker.Core.Models.Games;
using AirsoftMatchMaker.Core.Models.Players;
using AirsoftMatchMaker.Infrastructure.Data.Enums;

namespace AirsoftMatchMaker.Core.Models.Teams
{
    public class TeamViewModel
    {
        public TeamViewModel()
        {
            Players = new HashSet<PlayerMinModel>();
            Games = new HashSet<GameMinModel>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Wins { get; set; }
        public int Losses { get; set; }
        public SkillLevel AverageSkillLevel { get; set; }
        public ICollection<PlayerMinModel> Players { get; set; } = null!;

        public ICollection<GameMinModel> Games { get; set; }
    }
}
