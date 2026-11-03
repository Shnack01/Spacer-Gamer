using System;
using System.IO;
using System.Text.Json;

namespace SpacerGamer
{
    public static class SaveManager
    {
        private static readonly string SaveDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"SpacerGamer");
        private static readonly string SavePath = Path.Combine(SaveDir, "save.json");

        private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };

        public static void Save(PlayerData data)
        {
            try
            {
                Directory.CreateDirectory(SaveDir);

                // Write to a temp file first so a crash mid-write can't corrupt the real save
                string tmp = SavePath + ".tmp";
                File.WriteAllText(tmp, JsonSerializer.Serialize(data, Options));
                File.Move(tmp, SavePath, overwrite: true);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Save failed: {e.Message}");
            }
        }

        public static PlayerData Load()
        {
            try
            {
                if (File.Exists(SavePath))
                    return JsonSerializer.Deserialize<PlayerData>(File.ReadAllText(SavePath)) ?? new PlayerData();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Load failed: {e.Message}");
            }
            return new PlayerData();
        }
    }
}