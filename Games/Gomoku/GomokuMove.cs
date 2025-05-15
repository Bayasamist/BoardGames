using BoardGames.Core;

namespace BoardGames.Games.Gomoku
{
    public class GomokuMove : Move
    {
        public int Row { get; }
        public int Column { get; }

        public GomokuMove(Player player, int row, int column) : base(player)
        {
            Row = row;
            Column = column;
        }

        public override string ToString() => $"{Player.Name} places {Player.Symbol} at ({Row + 1}, {Column + 1})";
    }
}
