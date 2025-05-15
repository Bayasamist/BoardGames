using System;
using System.Collections.Generic;
using BoardGames.Core;

namespace BoardGames.Games.Gomoku
{
    public class Gomoku : Game
    {
        private GomokuBoard GomokuBoard => (GomokuBoard)Board;

        public Gomoku(List<Player> players) : base(new GomokuBoard(15, 15), players) { }

        public override bool IsValidMove(Move move)
        {
            if (move is GomokuMove m)
                return GomokuBoard.IsCellEmpty(m.Row, m.Column);
            return false;
        }

        protected override void ExecuteMove(Move move)
        {
            var m = (GomokuMove)move;
            GomokuBoard.PlaceSymbol(m.Row, m.Column, m.Player.Symbol.ToString());
        }

        protected override void UndoMove(Move move)
        {
            var m = (GomokuMove)move;
            GomokuBoard.UndoSymbol(m.Row, m.Column);
        }

        protected override void CheckGameOver()
        {
            if (MoveHistory.TryPeek(out Move lastMove) && lastMove is GomokuMove m)
            {
                if (GomokuBoard.HasFiveInRow(m.Row, m.Column, m.Player.Symbol.ToString()))
                {
                    IsGameOver = true;
                    Winner = m.Player;
                }
            }
        }

        public override List<Move> GetAvailableMoves()
        {
            var moves = new List<Move>();
            for (int r = 0; r < GomokuBoard.Rows; r++)
                for (int c = 0; c < GomokuBoard.Columns; c++)
                    if (GomokuBoard.IsCellEmpty(r, c))
                        moves.Add(new GomokuMove(CurrentPlayer, r, c));
            return moves;
        }

        public override GameState GetGameState() => null;
        public override void RestoreGameState(GameState gameState) { }

        public override string ToString() => GomokuBoard.ToString();
    }
}
