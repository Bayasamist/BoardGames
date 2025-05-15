using System;
using System.Text;
using BoardGames.Core;

namespace BoardGames.Games.TicTacToe
{
    public class NumericalTicTacToeBoard : Board
    {
        public int[,] SetGrid { get; private set; }
        public int TargetSum { get; private set; }
        public int Size => Rows;

        public NumericalTicTacToeBoard(int size) : base(size, size)
        {
            SetGrid = new int[size, size];
            TargetSum = size * (size * size + 1) / 2;
        }

        public override void Reset()
        {
            for (int row = 0; row < Size; row++)
                for (int col = 0; col < Size; col++)
                    SetGrid[row, col] = 0;
        }

        public bool IsValidMove(int row, int col)
        {
            return row >= 0 && row < Size && col >= 0 && col < Size && SetGrid[row, col] == 0;
        }

        public void PlaceNumber(int row, int col, int number)
        {
            SetGrid[row, col] = number;
        }

        public void ClearCell(int row, int col)
        {
            SetGrid[row, col] = 0;
        }

        public bool IsCellEmpty(int row, int col) => SetGrid[row, col] == 0;

        public bool IsFull()
        {
            for (int row = 0; row < Size; row++)
                for (int col = 0; col < Size; col++)
                    if (SetGrid[row, col] == 0)
                        return false;
            return true;
        }

        public bool CheckWin()
        {
            for (int i = 0; i < Size; i++)
                if (IsLineComplete(i, 0, 0, 1) || IsLineComplete(0, i, 1, 0))
                    return true;

            return IsLineComplete(0, 0, 1, 1) || IsLineComplete(0, Size - 1, 1, -1);
        }

        private bool IsLineComplete(int startRow, int startCol, int rowInc, int colInc)
        {
            int sum = 0;
            for (int i = 0; i < Size; i++)
            {
                int row = startRow + i * rowInc;
                int col = startCol + i * colInc;
                if (SetGrid[row, col] == 0) return false;
                sum += SetGrid[row, col];
            }
            return sum == TargetSum;
        }

        public void Display()
        {
            Console.WriteLine();
            Console.Write("   ");
            for (int col = 0; col < Size; col++)
                Console.Write($" {col + 1}  ");
            Console.WriteLine();

            for (int row = 0; row < Size; row++)
            {
                Console.Write("   ");
                for (int col = 0; col < Size; col++)
                    Console.Write("+---");
                Console.WriteLine("+");

                Console.Write($" {row + 1} ");
                for (int col = 0; col < Size; col++)
                {
                    int value = SetGrid[row, col];
                    string display = value == 0 ? " " : value.ToString();
                    if (value < 10 && value > 0)
                        display = " " + display;
                    Console.Write($"|{display} ");
                }
                Console.WriteLine("|");
            }

            Console.Write("   ");
            for (int col = 0; col < Size; col++)
                Console.Write("+---");
            Console.WriteLine("+");
        }
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine();
            sb.Append("   ");
            for (int col = 0; col < Size; col++)
                sb.Append($" {col + 1}  ");
            sb.AppendLine();

            for (int row = 0; row < Size; row++)
            {
                sb.Append("   ");
                for (int col = 0; col < Size; col++)
                    sb.Append("+---");
                sb.AppendLine("+");

                sb.Append($" {row + 1} ");
                for (int col = 0; col < Size; col++)
                {
                    int value = SetGrid[row, col];
                    string display = value == 0 ? " " : value.ToString();
                    if (value < 10 && value > 0)
                        display = " " + display;
                    sb.Append($"|{display} ");
                }
                sb.AppendLine("|");
            }

            sb.Append("   ");
            for (int col = 0; col < Size; col++)
                sb.Append("+---");
            sb.AppendLine("+");

            return sb.ToString();
        }


        public override Board Clone()
        {
            var clone = new NumericalTicTacToeBoard(Size);
            for (int row = 0; row < Size; row++)
                for (int col = 0; col < Size; col++)
                    clone.SetGrid[row, col] = this.SetGrid[row, col];
            return clone;
        }

    }
}
