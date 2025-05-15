using System;
using System.Collections.Generic;
using BoardGames.Core;
using BoardGames.UI;
using BoardGames.Utils;


namespace BoardGames
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Board Game Framework!");
            Console.WriteLine("=================================");
            Console.WriteLine();

            ShowMainMenu();

            Console.WriteLine("\nThank you for playing!");
        }

        static void ShowMainMenu()
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Main Menu");
                Console.WriteLine("=========");
                Console.WriteLine("1. Play Numerical Tic-Tac-Toe");
                Console.WriteLine("2. Play Notakto");
                Console.WriteLine("3. Play Gomoku");
                Console.WriteLine("4. Load Saved Game");
                Console.WriteLine("5. Exit");
                Console.WriteLine();
                Console.Write("Enter your choice (1-5): ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        SelectGameMode(GameType.NumericalTicTacToe);
                        break;
                    case "2":
                        SelectGameMode(GameType.Notakto);
                        break;
                    case "3":
                        SelectGameMode(GameType.Gomoku);
                        break;
                    case "4":
                        LoadSavedGame();
                        break;
                    case "5":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("\nInvalid choice. Press any key to continue...");
                        Console.ReadKey(true);
                        break;
                }
            }
        }

        static void SelectGameMode(GameType gameType)
        {
            Console.Clear();
            Console.WriteLine($"Select {gameType} Game Mode");
            Console.WriteLine("===================");
            Console.WriteLine("1. Human vs Human");
            Console.WriteLine("2. Human vs Computer");
            Console.WriteLine("3. Back to Main Menu");
            Console.WriteLine();
            Console.Write("Enter your choice (1-3): ");

            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    StartGame(gameType, GameMode.HumanVsHuman);
                    break;
                case "2":
                    StartGame(gameType, GameMode.HumanVsComputer);
                    break;
                case "3":
                    break;
                default:
                    Console.WriteLine("\nInvalid choice. Press any key to continue...");
                    Console.ReadKey(true);
                    SelectGameMode(gameType);
                    break;
            }
        }

        static void StartGame(GameType gameType, GameMode gameMode)
        {
            Game game = GameManager.CreateGame(gameType, gameMode);

            if (game == null)
            {
                Console.WriteLine("\nError creating game. Press any key to continue...");
                Console.ReadKey(true);
                return;
            }

            CommandHandler commandHandler = CreateCommandHandler(game, gameType);
            GameUI gameUI = new GameUI(game, commandHandler);

            gameUI.Run();
        }

        static CommandHandler CreateCommandHandler(Game game, GameType gameType)
        {
            switch (gameType)
            {
                case GameType.NumericalTicTacToe:
                    return new CommandHandler(game);
                case GameType.Notakto:
                    return new CommandHandler(game);
                case GameType.Gomoku:
                    return new CommandHandler(game);
                default:
                    return new CommandHandler(game);
            }
        }

        static void LoadSavedGame()
        {
            Console.Clear();
            Console.WriteLine("Load Saved Game");
            Console.WriteLine("==============");

            string[] savedGames = Utils.GameSerializer.GetSavedGames();

            if (savedGames.Length == 0)
            {
                Console.WriteLine("No saved games found.");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey(true);
                return;
            }

            for (int i = 0; i < savedGames.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {savedGames[i]}");
            }

            Console.WriteLine($"{savedGames.Length + 1}. Back to Main Menu");
            Console.WriteLine();
            Console.Write($"Enter your choice (1-{savedGames.Length + 1}): ");

            string input = Console.ReadLine();

            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= savedGames.Length)
            {
                string selectedGame = savedGames[choice - 1];
                LoadGame(selectedGame);
            }
            else if (choice == savedGames.Length + 1)
            {
                return;
            }
            else
            {
                Console.WriteLine("\nInvalid choice. Press any key to continue...");
                Console.ReadKey(true);
                LoadSavedGame();
            }
        }

        static void LoadGame(string saveName)
        {
            GameState gameState = Utils.GameSerializer.LoadGame(saveName);

            if (gameState == null)
            {
                Console.WriteLine($"\nError loading game '{saveName}'. Press any key to continue...");
                Console.ReadKey(true);
                return;
            }

            if (!Enum.TryParse<GameType>(gameState.GameType, out GameType gameType))
            {
                Console.WriteLine($"\nInvalid game type '{gameState.GameType}' in save file.");
                Console.WriteLine("Press any key to return to the main menu...");
                Console.ReadKey(true);
                return;
            }

            GameMode gameMode = gameState.Players.Count > 0 && gameState.Players[1].IsComputer
                ? GameMode.HumanVsComputer
                : GameMode.HumanVsHuman;

            Game game = GameManager.CreateGame(gameType, gameMode);

            if (game == null)
            {
                Console.WriteLine("\nError creating game. Press any key to continue...");
                Console.ReadKey(true);
                return;
            }

            game.RestoreGameState(gameState);

            CommandHandler commandHandler = CreateCommandHandler(game, gameType);
            GameUI gameUI = new GameUI(game, commandHandler);

            gameUI.Run();
        }
    }
}
