using System;
using System.Collections.Generic;
using BoardGames.Players;
using BoardGames.Games.Notakto;
using BoardGames.Games.Gomoku;
using BoardGames.Games.TicTacToe;
using BoardGames.Core;

namespace BoardGames.Core
{
    public static class GameManager
    {
        public static Game CreateGame(GameType gameType, GameMode gameMode)
        {
            switch (gameType)
            {
                case GameType.NumericalTicTacToe:
                    return CreateNumericalTicTacToe(gameMode);
                case GameType.Notakto:
                    return CreateNotakto(CreatePlayers(gameMode));
                case GameType.Gomoku:
                    return CreateGomoku(CreatePlayers(gameMode));
                default:
                    throw new ArgumentException($"Unknown game type: {gameType}");
            }
        }

        private static List<Player> CreatePlayers(GameMode gameMode)
        {
            List<Player> players = new List<Player>();

            switch (gameMode)
            {
                case GameMode.HumanVsHuman:
                    players.Add(new HumanPlayer("Player 1", "X"));
                    players.Add(new HumanPlayer("Player 2", "O"));
                    break;
                case GameMode.HumanVsComputer:
                    players.Add(new HumanPlayer("Player", "X"));
                    players.Add(new ComPlayer("Computer", "O"));
                    break;
                default:
                    throw new ArgumentException($"Unknown game mode: {gameMode}");
            }

            return players;
        }

        private static Game CreateNumericalTicTacToe(GameMode mode)
        {
            var player1 = new HumanPlayer("Player 1 (Odds)", "O")
            {
                AvailableNumbers = new List<int> { 1, 3, 5, 7, 9 }
            };

            Player player2 = mode == GameMode.HumanVsComputer
                ? new ComPlayer("Computer (Evens)", "E")
                {
                    AvailableNumbers = new List<int> { 2, 4, 6, 8 }
                }
                : new HumanPlayer("Player 2 (Evens)", "E")
                {
                    AvailableNumbers = new List<int> { 2, 4, 6, 8 }
                };

            return new NumericalTicTacToe(new List<Player> { player1, player2 });
        }

        private static Game CreateNotakto(List<Player> players)
        {
            return new Notakto(players);
        }

        private static Game CreateGomoku(List<Player> players)
        {
            return new Gomoku(players);
        }
    }

    public enum GameType
    {
        NumericalTicTacToe,
        Notakto,
        Gomoku
    }

    public enum GameMode
    {
        HumanVsHuman,
        HumanVsComputer
    }
}
