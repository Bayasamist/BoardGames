using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using BoardGames.Core;

namespace BoardGames.Utils
{
    public static class GameSerializer
    {
        private const string DefaultSaveDirectory = "saves";

        public static bool SaveGame(GameState gameState, string fileName, string directory = null)
        {
            try
            {
                string saveDir = directory ?? DefaultSaveDirectory;
                if (!Directory.Exists(saveDir))
                    Directory.CreateDirectory(saveDir);

                string filePath = Path.Combine(saveDir, $"{fileName}.json");
                string json = JsonSerializer.Serialize(gameState, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                File.WriteAllText(filePath, json);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static GameState LoadGame(string fileName, string directory = null)
        {
            try
            {
                string saveDir = directory ?? DefaultSaveDirectory;
                string filePath = Path.Combine(saveDir, $"{fileName}.json");

                if (!File.Exists(filePath))
                    return null;

                string json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<GameState>(json);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool SavedGameExists(string fileName, string directory = null)
        {
            string saveDir = directory ?? DefaultSaveDirectory;
            string filePath = Path.Combine(saveDir, $"{fileName}.json");
            return File.Exists(filePath);
        }

        public static string[] GetSavedGames(string directory = null)
        {
            try
            {
                string saveDir = directory ?? DefaultSaveDirectory;

                if (!Directory.Exists(saveDir))
                    return Array.Empty<string>();

                string[] files = Directory.GetFiles(saveDir, "*.json");
                for (int i = 0; i < files.Length; i++)
                {
                    files[i] = Path.GetFileNameWithoutExtension(files[i]);
                }

                return files;
            }
            catch (Exception)
            {
                return Array.Empty<string>();
            }
        }

        public static bool DeleteSavedGame(string fileName, string directory = null)
        {
            try
            {
                string saveDir = directory ?? DefaultSaveDirectory;
                string filePath = Path.Combine(saveDir, $"{fileName}.json");

                if (!File.Exists(filePath))
                    return false;

                File.Delete(filePath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
