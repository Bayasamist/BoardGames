using System;
using System.Collections.Generic;

namespace BoardGames.Core
{
    [Serializable]
    public class GameState
    {
        public string GameType { get; set; }
        public object BoardState { get; set; }
        public List<PlayerState> Players { get; set; }
        public int CurrentPlayerIndex { get; set; }
        public List<object> MoveHistory { get; set; }
        public List<object> RedoStack { get; set; }
        public bool IsGameOver { get; set; }
        public int WinnerIndex { get; set; }

        public GameState()
        {
            Players = new List<PlayerState>();
            MoveHistory = new List<object>();
            RedoStack = new List<object>();
            WinnerIndex = -1;
        }
    }

    [Serializable]
    public class PlayerState
    {
        public string Name { get; set; }
        public object Symbol { get; set; }
        public bool IsComputer { get; set; }
    }
}
