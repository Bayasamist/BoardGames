using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGames.Core;
using BoardGames.Core;

namespace BoardGames.UI
{
    public abstract class BoardRenderer
    {
        protected Board Board { get; set; }

        protected BoardRenderer(Board board)
        {
            Board = board;
        }

        public abstract string Render();

        public virtual void RenderToConsole()
        {
            Console.WriteLine(Render());
        }

        protected string CreateHorizontalBorder(int cellWidth)
        {
            StringBuilder border = new StringBuilder();
            border.Append('+');

            for (int i = 0; i < Board.Columns; i++)
            {
                border.Append(new string('-', cellWidth));
                border.Append('+');
            }

            return border.ToString();
        }

        protected string CenterText(string text, int width)
        {
            if (text.Length >= width)
                return text.Substring(0, width);

            int padding = width - text.Length;
            int leftPad = padding / 2;
            int rightPad = padding - leftPad;

            return new string(' ', leftPad) + text + new string(' ', rightPad);
        }
    }
}
