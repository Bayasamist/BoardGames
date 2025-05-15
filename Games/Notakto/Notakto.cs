using BoardGames.Core;
using System;
using System.Collections.Generic;

namespace BoardGames.Games.Notakto
{
    public class Notakto : Game
    {
        private NotaktoBoard NotaktoBoard => (NotaktoBoard)Board;

        public Notakto(List<Player> players)
            : base(new NotaktoBoard(), players) { }

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
                Winner = Players[(CurrentPlayerIndex + 1) % Players.Count]; // Opponent wins
            }
        }

        public override List<Move> GetAvailableMoves()
        {
            List<Move> moves = new();
            for (int boardIndex = 0; boardIndex < 3; boardIndex++)
                for (int row = 0; row < 3; row++)
                    for (int col = 0; col < 3; col++)
                        if (NotaktoBoard.IsValidMove(boardIndex, row, col))
                            moves.Add(new NotaktoMove(CurrentPlayer, boardIndex, row, col));
            return moves;
        }

        public override GameState GetGameState()
        {
            throw new NotImplementedException("GameState saving not implemented for Notakto.");
        }

        public override void RestoreGameState(GameState gameState)
        {
            throw new NotImplementedException("RestoreGameState is not yet supported for Notakto.");
        }

        public override string ToString()
        {
            return Board.ToString();
        }
    }
}
