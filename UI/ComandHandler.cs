using BoardGames.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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
            CurrentGame = game;
            ShouldExit = false;

            if (!Directory.Exists(SaveDirectory))
                Directory.CreateDirectory(SaveDirectory);
        }

        public virtual bool ProcessCommand(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
                return false;

            string[] parts = command.Trim().ToLower().Split(' ');
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
            Console.WriteLine("  help      - Show this help information");
            Console.WriteLine("  undo      - Undo the last move");
            Console.WriteLine("  redo      - Redo the last undone move");
            Console.WriteLine("  save [name] - Save the current game state (default name: 'default')");
            Console.WriteLine("  load [name] - Load a saved game state (default name: 'default')");
            Console.WriteLine("  exit      - Exit the game");
            Console.WriteLine("\nGame-Specific Commands:");
            ShowGameHelp();
            Console.WriteLine();
        }

        protected virtual void ShowGameHelp()
        {
        }

        protected bool Undo()
        {
            bool result = CurrentGame.Undo();
            if (result)
                Console.WriteLine("Move undone.");
            else
                Console.WriteLine("No moves to undo.");
            return result;
        }

        protected bool Redo()
        {
            bool result = CurrentGame.Redo();
            if (result)
                Console.WriteLine("Move redone.");
            else
                Console.WriteLine("No moves to redo.");
            return result;
        }

        protected bool Save(string name)
        {
            try
            {
                GameState gameState = CurrentGame.GetGameState();
                string json = JsonSerializer.Serialize(gameState);
                string filePath = Path.Combine(SaveDirectory, $"{name}.json");
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
                GameState gameState = JsonSerializer.Deserialize<GameState>(json);
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
