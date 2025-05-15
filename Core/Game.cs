using System;
using System.Collections.Generic;

namespace BoardGames.Core
{

    public abstract class Game
    {
        protected Board Board { get; set; }
        public List<Player> Players { get; protected set; }

        protected int CurrentPlayerIndex { get; set; }
        protected Stack<Move> MoveHistory { get; set; } = new();
        protected Stack<Move> RedoStack { get; set; } = new();
        public bool IsGameOver { get; protected set; }
        public Player Winner { get; protected set; }

        public Player CurrentPlayer => Players[CurrentPlayerIndex];

        protected Game(Board board, List<Player> players)
        {
            Board = board ?? throw new ArgumentNullException(nameof(board));
            Players = players ?? throw new ArgumentNullException(nameof(players));
            if (players.Count == 0) throw new ArgumentException("At least one player is required.");
            CurrentPlayerIndex = 0;
            IsGameOver = false;
            Winner = null;
        }

        public virtual bool MakeMove(Move move)
        {
            if (IsGameOver || move == null || !IsValidMove(move))
                return false;

            ExecuteMove(move);
            MoveHistory.Push(move);
            RedoStack.Clear();
            CheckGameOver();

            if (!IsGameOver)
                SwitchPlayer();

            return true;
        }

        public abstract bool IsValidMove(Move move);
        protected abstract void ExecuteMove(Move move);
        protected abstract void UndoMove(Move move);
        protected abstract void CheckGameOver();

        protected virtual void SwitchPlayer()
        {
            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % Players.Count;
        }

        public virtual bool Undo()
        {
            if (MoveHistory.Count == 0)
                return false;

            var lastMove = MoveHistory.Pop();
            RedoStack.Push(lastMove);
            UndoMove(lastMove);
            IsGameOver = false;
            Winner = null;
            CurrentPlayerIndex = (CurrentPlayerIndex - 1 + Players.Count) % Players.Count;

            return true;
        }

        public virtual bool Redo()
        {
            if (RedoStack.Count == 0)
                return false;

            var move = RedoStack.Pop();
            ExecuteMove(move);
            MoveHistory.Push(move);
            CheckGameOver();

            if (!IsGameOver)
                SwitchPlayer();

            return true;
        }

        public abstract GameState GetGameState();
        public abstract void RestoreGameState(GameState gameState);
        public abstract List<Move> GetAvailableMoves();

        public override string ToString() => Board.ToString();
    }
}
