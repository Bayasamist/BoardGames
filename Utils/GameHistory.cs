using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGames.Core;

namespace BoardGames.Utils
{
    public class GameHistory
    {
        private Stack<Move> MoveHistory { get; } = new Stack<Move>();
        private Stack<Move> RedoStack { get; } = new Stack<Move>();

        public void RecordMove(Move move)
        {
            MoveHistory.Push(move);
            RedoStack.Clear();
        }

        public Move GetLastMove()
        {
            if (MoveHistory.Count == 0)
                return null;

            return MoveHistory.Peek();
        }

        public Move Undo()
        {
            if (MoveHistory.Count == 0)
                return null;

            Move lastMove = MoveHistory.Pop();
            RedoStack.Push(lastMove);
            return lastMove;
        }

        public Move Redo()
        {
            if (RedoStack.Count == 0)
                return null;

            Move lastUndoneMove = RedoStack.Pop();
            MoveHistory.Push(lastUndoneMove);
            return lastUndoneMove;
        }

        public bool CanUndo()
        {
            return MoveHistory.Count > 0;
        }

        public bool CanRedo()
        {
            return RedoStack.Count > 0;
        }

        public List<Move> GetAllMoves()
        {
            Move[] movesArray = MoveHistory.ToArray();
            List<Move> moves = new List<Move>(movesArray);
            moves.Reverse();
            return moves;
        }

        public List<Move> GetAllRedoMoves()
        {
            Move[] movesArray = RedoStack.ToArray();
            List<Move> moves = new List<Move>(movesArray);
            moves.Reverse();
            return moves;
        }

        public void Clear()
        {
            MoveHistory.Clear();
            RedoStack.Clear();
        }
    }
}
