using BoardGames.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.IO;

namespace BoardGames.UI
{
    public class CommandHandler
    {
        protected Game CurrentGame { get; set; }
        public bool ShouldExit { get; protected set; }
        protected const string SaveDirectory = "saves";

        public CommandHandler(Game game)
        {
            CurrentGame = game ?? throw new ArgumentNullException(nameof(game));
            ShouldExit = false;

            if (!Directory.Exists(SaveDirectory))
                Directory.CreateDirectory(SaveDirectory);
        }

        public virtual bool ProcessCommand(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
                return false;

            string[] parts = command.Trim().ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string action = parts[0];

            switch (action)
            {
                case "help":
                    ShowHelp();
                    return true;
                case "undo":
                    return Undo();
                case "redo":
                    return Redo();
                case "save":
                    return parts.Length > 1 ? Save(parts[1]) : Save("default");
                case "load":
                    return parts.Length > 1 ? Load(parts[1]) : Load("default");
                case "exit":
                    ShouldExit = true;
                    return true;
                default:
                    return ProcessGameCommand(command);
            }
        }

        protected virtual bool ProcessGameCommand(string command)
        {
            return false;
        }

        protected virtual void ShowHelp()
        {
            Console.WriteLine("\n=== GAME HELP ===");
            Console.WriteLine("General Commands:");
            Console.WriteLine("  help           - Show this help menu");
            Console.WriteLine("  undo           - Undo the last move");
            Console.WriteLine("  redo           - Redo the last undone move");
            Console.WriteLine("  save [name]    - Save the game (default: 'default')");
            Console.WriteLine("  load [name]    - Load a saved game (default: 'default')");
            Console.WriteLine("  exit           - Exit the game");
            Console.WriteLine("\nGame-Specific Commands:");
            ShowGameHelp();
            Console.WriteLine();
        }

        protected virtual void ShowGameHelp()
        {
            // Override to add custom game instructions
        }

        protected bool Undo()
        {
            bool result = CurrentGame.Undo();
            Console.WriteLine(result ? "Move undone." : "No moves to undo.");
            return result;
        }

        protected bool Redo()
        {
            bool result = CurrentGame.Redo();
            Console.WriteLine(result ? "Move redone." : "No moves to redo.");
            return result;
        }

        protected bool Save(string name)
        {
            try
            {
                GameState gameState = CurrentGame.GetGameState();
                if (gameState == null) throw new InvalidOperationException("GameState is null.");

                string filePath = Path.Combine(SaveDirectory, $"{name}.json");
                string json = JsonSerializer.Serialize(gameState, new JsonSerializerOptions { WriteIndented = true });

                File.WriteAllText(filePath, json);
                Console.WriteLine($"Game saved as '{name}'.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving game: {ex.Message}");
                return false;
            }
        }

        protected bool Load(string name)
        {
            try
            {
                string filePath = Path.Combine(SaveDirectory, $"{name}.json");
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"Save file '{name}' not found.");
                    return false;
                }

                string json = File.ReadAllText(filePath);
                var gameState = JsonSerializer.Deserialize<GameState>(json);

                if (gameState == null)
                {
                    Console.WriteLine("Failed to deserialize game state.");
                    return false;
                }

                CurrentGame.RestoreGameState(gameState);
                Console.WriteLine($"Game '{name}' loaded successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading game: {ex.Message}");
                return false;
            }
        }
    }
}
