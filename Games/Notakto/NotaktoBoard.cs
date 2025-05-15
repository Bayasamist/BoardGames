using System;
using System.Text;
using BoardGames.Core;

namespace BoardGames.Games.Notakto
{
    public class NotaktoBoard : Board
    {
        public char[][,] Boards { get; private set; } = new char[3][,];
        public bool[] Completed { get; private set; } = new bool[3];

        public NotaktoBoard() : base(3, 3)
        {
            for (int i = 0; i < 3; i++)
            {
                Boards[i] = new char[3, 3];
                for (int r = 0; r < 3; r++)
                    for (int c = 0; c < 3; c++)
                        Boards[i][r, c] = ' ';
            }
        }

        public override void Reset()
        {
            for (int i = 0; i < 3; i++)
            {
                Completed[i] = false;
                for (int r = 0; r < 3; r++)
                    for (int c = 0; c < 3; c++)
                        Boards[i][r, c] = ' ';
            }
        }

        public override Board Clone()
        {
            var clone = new NotaktoBoard();
            for (int i = 0; i < 3; i++)
            {
                Array.Copy(this.Boards[i], clone.Boards[i], 9);
                clone.Completed[i] = this.Completed[i];
            }
            return clone;
        }

        public bool IsValidMove(int b, int r, int c)
        {
            return !Completed[b] && Boards[b][r, c] == ' ';
        }

        public void ApplyMove(int b, int r, int c)
        {
            Boards[b][r, c] = 'X';
            if (CheckThreeInRow(Boards[b]))
                Completed[b] = true;
        }

        public void UndoMove(int b, int r, int c)
        {
            Boards[b][r, c] = ' ';
            Completed[b] = false;
        }

        public bool IsGameOver() => Completed[0] && Completed[1] && Completed[2];

        private bool CheckThreeInRow(char[,] board)
        {
            for (int i = 0; i < 3; i++)
                if (board[i, 0] == 'X' && board[i, 1] == 'X' && board[i, 2] == 'X') return true;
            for (int j = 0; j < 3; j++)
                if (board[0, j] == 'X' && board[1, j] == 'X' && board[2, j] == 'X') return true;
            if (board[0, 0] == 'X' && board[1, 1] == 'X' && board[2, 2] == 'X') return true;
            if (board[0, 2] == 'X' && board[1, 1] == 'X' && board[2, 0] == 'X') return true;
            return false;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("========== GOMOKU ==========");
            sb.AppendLine("Enter your move as: board row col (e.g. 1 8 8)");
            sb.AppendLine("First to get 3 in a row on any board wins!\n");

            for (int b = 0; b < 3; b++)
            {
                sb.AppendLine($"Board {b + 1} {(Completed[b] ? "[COMPLETED]" : "")}");
                sb.Append("    1   2   3\n");
                sb.Append("  +---+---+---+\n");

                for (int r = 0; r < 3; r++)
                {
                    sb.Append($"{r + 1} ");
                    for (int c = 0; c < 3; c++)
                    {
                        char value = Boards[b][r, c];
                        sb.Append($"| {(value == ' ' ? ' ' : value)} ");
                    }
                    sb.Append("|\n");
                    sb.Append("  +---+---+---+\n");
                }

                if (b < 2)
                    sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
