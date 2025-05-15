using System;
using System.Collections.Generic;
using BoardGames.Core;

namespace BoardGames.Games.Gomoku
{
    public class Gomoku : Game
    {
        private GomokuBoard GomokuBoard => (GomokuBoard)Board;

        public Gomoku(List<Player> players)
            : base(new GomokuBoard(15, 15), players) { }

        public override bool IsValidMove(Move move)
        {
            if (move is GomokuMove gm)
                return GomokuBoard.IsCellEmpty(gm.Row, gm.Column);
            return false;
        }

        protected override void ExecuteMove(Move move)
        {
            var gm = (GomokuMove)move;
            GomokuBoard.PlaceSymbol(gm.Row, gm.Column, gm.Player.Symbol.ToString());
        }

        protected override void UndoMove(Move move)
        {
            var gm = (GomokuMove)move;
            GomokuBoard.UndoSymbol(gm.Row, gm.Column);
        }

        protected override void CheckGameOver()
        {
            if (MoveHistory.TryPeek(out Move lastMove) && lastMove is GomokuMove gm)
            {
                if (GomokuBoard.HasFiveInRow(gm.Row, gm.Column, gm.Player.Symbol.ToString()))
                {
                    IsGameOver = true;
                    Winner = gm.Player;
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

        public override GameState GetGameState() => throw new NotImplementedException();
        public override void RestoreGameState(GameState gameState) => throw new NotImplementedException();

        public override string ToString() => GomokuBoard.ToString();
    }
}
