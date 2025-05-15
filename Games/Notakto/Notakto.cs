using BoardGames.Core;
using System;
using System.Collections.Generic;

namespace BoardGames.Games.Notakto
{
    public class Notakto : Game
    {
        private NotaktoBoard NotaktoBoard => (NotaktoBoard)Board;

        public Notakto(List<Player> players) : base(new NotaktoBoard(), players) { }

        public override bool IsValidMove(Move move)
        {
            if (move is NotaktoMove m)
                return NotaktoBoard.IsValidMove(m.BoardIndex, m.Row, m.Column);
            return false;
        }

        protected override void ExecuteMove(Move move)
        {
            var m = (NotaktoMove)move;
            NotaktoBoard.ApplyMove(m.BoardIndex, m.Row, m.Column);
        }

        protected override void UndoMove(Move move)
        {
            var m = (NotaktoMove)move;
            NotaktoBoard.UndoMove(m.BoardIndex, m.Row, m.Column);
        }

        protected override void CheckGameOver()
        {
            if (NotaktoBoard.IsGameOver())
            {
                IsGameOver = true;
                Winner = Players[(CurrentPlayerIndex + 1) % Players.Count]; 
            }
        }

        public override List<Move> GetAvailableMoves()
        {
            List<Move> moves = new List<Move>();
            for (int b = 0; b < 3; b++)
                for (int r = 0; r < 3; r++)
                    for (int c = 0; c < 3; c++)
                        if (NotaktoBoard.IsValidMove(b, r, c))
                            moves.Add(new NotaktoMove(CurrentPlayer, b, r, c));
            return moves;
        }

        public override GameState GetGameState() => null;
        public override void RestoreGameState(GameState gameState) { }

        public override string ToString()
        {
            return Board.ToString();
        }
    }
}
