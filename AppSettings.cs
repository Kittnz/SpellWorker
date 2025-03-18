using System;
using System.IO;
using System.Text.Json;

namespace SpellWorker
{
    public class AppSettings
    {
        private static AppSettings _instance;
        private static readonly string SettingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "SpellWorker",
            "settings.json");

        public string DbServer { get; set; } = "localhost";
        public string DbName { get; set; } = "tw_dev_world";
        public string DbUsername { get; set; } = "root";
        public string DbPassword { get; set; } = "root";
        public int DbPort { get; set; } = 3310;

        private AppSettings() { }

        public static AppSettings Default
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Load();
                }
                return _instance;
            }
        }

        public void Save()
        {
            try
            {
                // Ensure directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath));

                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(this, options);
                File.WriteAllText(SettingsPath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving settings: {ex.Message}");
            }
        }

        private static AppSettings Load()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    string json = File.ReadAllText(SettingsPath);
                    var settings = JsonSerializer.Deserialize<AppSettings>(json);
                    return settings ?? new AppSettings();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading settings: {ex.Message}");
            }

            return new AppSettings();
        }
    }
}