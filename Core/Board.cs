namespace BoardGames.Core
{
   
    public abstract class Board
    {
        public int Rows { get; protected set; }
        public int Columns { get; protected set; }

        protected Board(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
        }

    
        public virtual bool IsValidPosition(int row, int column)
        {
            return row >= 0 && row < Rows && column >= 0 && column < Columns;
        }

        public abstract Board Clone();
        public abstract void Reset();
        public abstract override string ToString();
    }
}
