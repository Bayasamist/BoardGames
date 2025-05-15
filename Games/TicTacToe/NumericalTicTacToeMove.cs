using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGames.Core;

namespace BoardGames.Games.TicTacToe
{
    public class NumericalTicTacToeMove : Move
    {
        public int Row { get; }
        public int Col { get; }
        public int Number { get; }

        public NumericalTicTacToeMove(Player player, int row, int col, int number) : base(player)
        {
            Row = row;
            Col = col;
            Number = number;
        }

        public override string ToString()
        {
            return $"{Player.Name} places {Number} at ({Row + 1}, {Col + 1})";
        }
    }
}