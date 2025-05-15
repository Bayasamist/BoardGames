using BoardGames.Core;
using System;
using System.Collections.Generic;

namespace BoardGames.Games.TicTacToe
{
    public class NumericalTicTacToe : Game
    {
        private NumericalTicTacToeBoard NumBoard => (NumericalTicTacToeBoard)Board;

        public NumericalTicTacToe(List<Player> players) : base(new NumericalTicTacToeBoard(3), players) { }

        public override bool IsValidMove(Move move)
        {
            if (move is NumericalTicTacToeMove m)
            {
                if (CurrentPlayer is Player playerWithNumbers &&
                    playerWithNumbers.GetType().GetProperty("AvailableNumbers")?.GetValue(playerWithNumbers) is List<int> availableNumbers)
                {
                    return NumBoard.IsCellEmpty(m.Row, m.Col) && availableNumbers.Contains(m.Number);
                }
            }
            return false;
        }

        protected override void ExecuteMove(Move move)
        {
            var m = (NumericalTicTacToeMove)move;
            NumBoard.PlaceNumber(m.Row, m.Col, m.Number);

            if (CurrentPlayer.GetType().GetProperty("AvailableNumbers")?.GetValue(CurrentPlayer) is List<int> availableNumbers)
            {
                availableNumbers.Remove(m.Number);
            }
        }

        protected override void UndoMove(Move move)
        {
            var m = (NumericalTicTacToeMove)move;
            NumBoard.ClearCell(m.Row, m.Col);

            if (CurrentPlayer.GetType().GetProperty("AvailableNumbers")?.GetValue(CurrentPlayer) is List<int> availableNumbers)
            {
                availableNumbers.Add(m.Number);
            }
        }

        protected override void CheckGameOver()
        {
            if (NumBoard.CheckWin())
            {
                IsGameOver = true;
                Winner = CurrentPlayer;
            }
            else if (NumBoard.IsFull())
            {
                IsGameOver = true;
                Winner = null;
            }
        }

        public override List<Move> GetAvailableMoves()
        {
            var moves = new List<Move>();

            if (CurrentPlayer.GetType().GetProperty("AvailableNumbers")?.GetValue(CurrentPlayer) is List<int> availableNumbers)
            {
                foreach (int number in availableNumbers)
                {
                    for (int r = 0; r < NumBoard.Size; r++)
                    {
                        for (int c = 0; c < NumBoard.Size; c++)
                        {
                            if (NumBoard.IsCellEmpty(r, c))
                                moves.Add(new NumericalTicTacToeMove(CurrentPlayer, r, c, number));
                        }
                    }
                }
            }

            return moves;
        }

        public override GameState GetGameState() => null;

        public override void RestoreGameState(GameState gameState) { }

        public override string ToString()
        {
            return NumBoard.ToString();
        }
    }
}