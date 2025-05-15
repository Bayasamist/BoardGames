using BoardGames.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using BoardGames.Games.TicTacToe;
using BoardGames.Games.Notakto;
using BoardGames.Games.Gomoku;

namespace BoardGames.Players
{
    public class ComPlayer : Player
    {
        public List<int> AvailableNumbers { get; set; } = new List<int>();

        public ComPlayer(string name, object symbol)
            : base(name, symbol, isComputer: true) { }

        public override Move GetMove(Game game)
        {
            if (game is Notakto notaktoGame)
            {
                var moves = notaktoGame.GetAvailableMoves();
                var strategic = moves.GroupBy(m => ((NotaktoMove)m).BoardIndex)
                                      .OrderBy(g => g.Count())
                                      .FirstOrDefault();
                return strategic?.FirstOrDefault();
            }

            if (game is Gomoku gomokuGame)
            {
                var moves = gomokuGame.GetAvailableMoves();
                var board = gomokuGame.GetType()
                    .GetProperty("Board", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.GetValue(gomokuGame) as GomokuBoard;

                // Try to win
                foreach (var move in moves.Cast<GomokuMove>())
                {
                    board.PlaceSymbol(move.Row, move.Column, this.Symbol.ToString());
                    if (board.HasFiveInRow(move.Row, move.Column, this.Symbol.ToString()))
                    {
                        board.UndoSymbol(move.Row, move.Column);
                        return move;
                    }
                    board.UndoSymbol(move.Row, move.Column);
                }

                // Try to block
                var opponent = game.Players.FirstOrDefault(p => p != this);
                string opponentSymbol = opponent?.Symbol.ToString();
                foreach (var move in moves.Cast<GomokuMove>())
                {
                    board.PlaceSymbol(move.Row, move.Column, opponentSymbol);
                    if (board.HasFiveInRow(move.Row, move.Column, opponentSymbol))
                    {
                        board.UndoSymbol(move.Row, move.Column);
                        return move;
                    }
                    board.UndoSymbol(move.Row, move.Column);
                }

                // Take center if free
                int center = board.Rows / 2;
                if (board.IsCellEmpty(center, center))
                {
                    return new GomokuMove(this, center, center);
                }

                return moves.FirstOrDefault();
            }

            if (game is NumericalTicTacToe ttt)
            {
                var board = ttt.GetType()
                    .GetProperty("Board", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.GetValue(ttt) as NumericalTicTacToeBoard;

                if (board == null || AvailableNumbers.Count == 0)
                    return null;

                var opponent = game.Players.FirstOrDefault(p => p != this);
                var opponentNumbers = (opponent as HumanPlayer)?.AvailableNumbers
                                   ?? (opponent as ComPlayer)?.AvailableNumbers
                                   ?? new List<int>();

                foreach (var number in AvailableNumbers)
                {
                    for (int row = 0; row < board.Size; row++)
                    {
                        for (int col = 0; col < board.Size; col++)
                        {
                            if (!board.IsCellEmpty(row, col)) continue;

                            board.PlaceNumber(row, col, number);
                            bool wins = board.CheckWin();
                            board.ClearCell(row, col);

                            if (wins)
                                return new NumericalTicTacToeMove(this, row, col, number);
                        }
                    }
                }

                foreach (var number in opponentNumbers)
                {
                    for (int row = 0; row < board.Size; row++)
                    {
                        for (int col = 0; col < board.Size; col++)
                        {
                            if (!board.IsCellEmpty(row, col)) continue;

                            board.PlaceNumber(row, col, number);
                            bool wouldWin = board.CheckWin();
                            board.ClearCell(row, col);

                            if (wouldWin)
                            {
                                foreach (var myNumber in AvailableNumbers)
                                {
                                    return new NumericalTicTacToeMove(this, row, col, myNumber);
                                }
                            }
                        }
                    }
                }

                int ticCenter = board.Size / 2;
                if (ticCenter >= 0 && ticCenter < board.Size && board.IsCellEmpty(ticCenter, ticCenter))
                {
                    return new NumericalTicTacToeMove(this, ticCenter, ticCenter, AvailableNumbers[0]);
                }

                foreach (var number in AvailableNumbers)
                {
                    for (int row = 0; row < board.Size; row++)
                    {
                        for (int col = 0; col < board.Size; col++)
                        {
                            if (board.IsCellEmpty(row, col))
                                return new NumericalTicTacToeMove(this, row, col, number);
                        }
                    }
                }
            }

            return null;
        }
    }
}
