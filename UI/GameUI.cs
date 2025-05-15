using System;
using System.Threading;
using BoardGames.Core;
using BoardGames.Games.Gomoku;
using BoardGames.Games.Notakto;
using BoardGames.Games.TicTacToe;

namespace BoardGames.UI
{
    public class GameUI
    {
        protected Game CurrentGame { get; set; }
        protected CommandHandler CommandHandler { get; set; }

        public GameUI(Game game, CommandHandler commandHandler)
        {
            CurrentGame = game;
            CommandHandler = commandHandler;
        }

        public void Run()
        {
            DisplayWelcomeMessage();

            while (!CommandHandler.ShouldExit)
            {
                DisplayGame();
                HandlePlayerTurn();

                if (CurrentGame.IsGameOver)
                {
                    DisplayGameOver();
                    break;
                }
            }

            DisplayExitMessage();
        }

        protected virtual void DisplayWelcomeMessage()
        {
            Console.Clear();
            Console.WriteLine("Welcome to the Board Game Framework!");
            Console.WriteLine("Type 'help' for a list of available commands.");
            Console.WriteLine();
        }

        protected virtual void DisplayGame()
        {
            Console.Clear();
            Console.WriteLine(CurrentGame.ToString());
            Console.WriteLine($"Current player: {CurrentGame.CurrentPlayer}");
            Console.WriteLine();
        }

        protected virtual void HandlePlayerTurn()
        {
            if (CurrentGame.CurrentPlayer.IsComputer)
            {
                HandleComputerTurn();
            }
            else
            {
                HandleHumanTurn();
            }
        }

        protected virtual void HandleHumanTurn()
        {
            bool validCommand = false;

            while (!validCommand && !CommandHandler.ShouldExit)
            {
                Console.Write($"{CurrentGame.CurrentPlayer.Name}'s turn > ");
                string input = Console.ReadLine();

                if (CurrentGame is Notakto)
                {
                    string[] parts = input?.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (parts?.Length == 3 &&
                        int.TryParse(parts[0], out int board) &&
                        int.TryParse(parts[1], out int row) &&
                        int.TryParse(parts[2], out int col))
                    {
                        var move = new NotaktoMove(CurrentGame.CurrentPlayer, board - 1, row - 1, col - 1);
                        validCommand = CurrentGame.MakeMove(move);
                        if (!validCommand)
                            Console.WriteLine("Invalid move. Try again.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid format. Use: board row col (e.g. 1 2 3)");
                    }
                }
                else if (CurrentGame is Gomoku)
                {
                    string[] parts = input?.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (parts?.Length == 2 &&
                        int.TryParse(parts[0], out int row) &&
                        int.TryParse(parts[1], out int col))
                    {
                        var move = new GomokuMove(CurrentGame.CurrentPlayer, row - 1, col - 1);
                        validCommand = CurrentGame.MakeMove(move);
                        if (!validCommand)
                            Console.WriteLine("Invalid move. Try again.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid format. Use: row col (e.g. 7 9)");
                    }
                }
                else if (CurrentGame is NumericalTicTacToe)
                {
                    string[] parts = input?.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (parts?.Length == 3 &&
                        int.TryParse(parts[0], out int row) &&
                        int.TryParse(parts[1], out int col) &&
                        int.TryParse(parts[2], out int number))
                    {
                        var move = new NumericalTicTacToeMove(CurrentGame.CurrentPlayer, row - 1, col - 1, number);
                        validCommand = CurrentGame.MakeMove(move);
                        if (!validCommand)
                            Console.WriteLine("Invalid move. Try again.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid format. Use: row col number (e.g. 1 2 3)");
                    }
                }
                else
                {
                    validCommand = CommandHandler.ProcessCommand(input);
                    if (!validCommand && !CommandHandler.ShouldExit)
                        Console.WriteLine("Invalid command. Type 'help' for a list of available commands.");
                }
            }
        }

        protected virtual void HandleComputerTurn()
        {
            Console.WriteLine($"{CurrentGame.CurrentPlayer.Name} is thinking...");
            Thread.Sleep(1000);

            Move move = CurrentGame.CurrentPlayer.GetMove(CurrentGame);
            if (move != null)
            {
                Console.WriteLine($"{CurrentGame.CurrentPlayer.Name} plays: {move}");
                CurrentGame.MakeMove(move);
                Thread.Sleep(1000);
            }
            else
            {
                Console.WriteLine($"{CurrentGame.CurrentPlayer.Name} cannot make a move.");
                Thread.Sleep(1000);
            }
        }

        protected virtual void DisplayGameOver()
        {
            Console.Clear();
            Console.WriteLine(CurrentGame.ToString());
            Console.WriteLine("Game over!");

            if (CurrentGame.Winner != null)
            {
                Console.WriteLine($"{CurrentGame.Winner.Name} wins!");
            }
            else
            {
                Console.WriteLine("It's a draw!");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey(true);
        }

        protected virtual void DisplayExitMessage()
        {
            Console.Clear();
            Console.WriteLine("Thank you for playing!");
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey(true);
        }
    }
}