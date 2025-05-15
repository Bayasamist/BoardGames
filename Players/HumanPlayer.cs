using BoardGames.Core;
using System;
using System.Collections.Generic;

namespace BoardGames.Players
{
    public class HumanPlayer : Player
    {
        // Required for Numerical Tic-Tac-Toe
        public List<int> AvailableNumbers { get; set; } = new List<int>();

        public HumanPlayer(string name, object symbol) : base(name, symbol, isComputer: false) { }

        public override Move GetMove(Game game)
        {
            // Handled via GameUI
            return null;
        }
    }
}
