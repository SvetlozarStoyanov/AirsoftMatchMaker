﻿using AirsoftMatchMaker.Infrastructure.Data.Enums;

namespace AirsoftMatchMaker.Core.Models.Games
{
    public class GameMinModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Odds { get; set; }
        public string Result { get; set; }
        public string Date { get; set; }
        public GameStatus GameStatus { get; set; }
    }
}
