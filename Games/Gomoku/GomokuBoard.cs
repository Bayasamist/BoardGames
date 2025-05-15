using System;
using System.Text;
using BoardGames.Core;

namespace BoardGames.Games.Gomoku
{
    public class GomokuBoard : Board
    {
        public string[,] Grid { get; private set; }

        public GomokuBoard(int rows, int cols) : base(rows, cols)
        {
            Grid = new string[rows, cols];
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                    Grid[r, c] = "";
        }

        public override void Reset()
        {
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    Grid[r, c] = "";
        }

        public override Board Clone()
        {
            var copy = new GomokuBoard(Rows, Columns);
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    copy.Grid[r, c] = this.Grid[r, c];
            return copy;
        }

        public bool IsCellEmpty(int r, int c) => string.IsNullOrEmpty(Grid[r, c]);

        public void PlaceSymbol(int r, int c, string symbol) => Grid[r, c] = symbol;

        public void UndoSymbol(int r, int c) => Grid[r, c] = "";

        public bool HasFiveInRow(int row, int col, string symbol)
        {
            return CheckDirection(row, col, 1, 0, symbol) ||
                   CheckDirection(row, col, 0, 1, symbol) ||
                   CheckDirection(row, col, 1, 1, symbol) ||
                   CheckDirection(row, col, 1, -1, symbol);
        }

        private bool CheckDirection(int row, int col, int dr, int dc, string symbol)
        {
            int count = 1;

            for (int d = 1; d < 5; d++)
            {
                int r = row + dr * d, c = col + dc * d;
                if (r >= 0 && r < Rows && c >= 0 && c < Columns && Grid[r, c] == symbol)
                    count++;
                else break;
            }

            for (int d = 1; d < 5; d++)
            {
                int r = row - dr * d, c = col - dc * d;
                if (r >= 0 && r < Rows && c >= 0 && c < Columns && Grid[r, c] == symbol)
                    count++;
                else break;
            }

            return count >= 5;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("========== GOMOKU ==========");
            sb.AppendLine("Enter your move as:  row col (e.g. 8 8)");
            sb.AppendLine("First to get 5 in a row on any board wins!\n");

            sb.Append("    ");
            for (int c = 0; c < Columns; c++)
                sb.Append($" {c + 1,2} "); 
            sb.AppendLine();

            sb.Append("   +");
            for (int c = 0; c < Columns; c++)
                sb.Append("---+");
            sb.AppendLine();

            for (int r = 0; r < Rows; r++)
            {
                sb.Append($"{r + 1,2} |"); 

                for (int c = 0; c < Columns; c++)
                {
                    string val = string.IsNullOrWhiteSpace(Grid[r, c]) ? "." : Grid[r, c];
                    sb.Append($" {val} |");
                }

                sb.AppendLine();
                sb.Append("   +");
                for (int c = 0; c < Columns; c++)
                    sb.Append("---+");
                sb.AppendLine();
            }

            return sb.ToString();
        }

    }
}