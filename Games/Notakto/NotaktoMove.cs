using BoardGames.Core;

namespace BoardGames.Games.Notakto
{
    public class NotaktoMove : Move
    {
        public int BoardIndex { get; }
        public int Row { get; }
        public int Column { get; }

        public NotaktoMove(Player player, int boardIndex, int row, int column) : base(player)
        {
            BoardIndex = boardIndex;
            Row = row;
            Column = column;
        }

        public override string ToString()
        {
            return $"{Player.Name} places X at board {BoardIndex + 1}, ({Row + 1}, {Column + 1})";
        }
    }
}
