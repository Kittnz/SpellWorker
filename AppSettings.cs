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

        public string DbServer { get; set; } = "127.0.0.1";
        public string DbName { get; set; } = "tw_dev_world";
        public string DbUsername { get; set; } = "root";
        public string DbPassword { get; set; } = "root";
        public int DbPort { get; set; } = 3310;

        private AppSettings()
        {
            // Try to load from the generated settings first
            try
            {
                var generatedSettings = SpellEditor.Settings.Default;
                DbServer = generatedSettings.DbServer;
                DbName = generatedSettings.DbName;
                DbUsername = generatedSettings.DbUsername;
                DbPassword = generatedSettings.DbPassword;
                DbPort = generatedSettings.DbPort;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not load from generated settings: {ex.Message}");
                // Use defaults defined above
            }
        }

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
                // Save to JSON file
                Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath));
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(this, options);
                File.WriteAllText(SettingsPath, json);

                // Also save to the generated settings
                try
                {
                    var generatedSettings = SpellEditor.Settings.Default;
                    generatedSettings.DbServer = DbServer;
                    generatedSettings.DbName = DbName;
                    generatedSettings.DbUsername = DbUsername;
                    generatedSettings.DbPassword = DbPassword;
                    generatedSettings.DbPort = DbPort;
                    generatedSettings.Save();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could not save to generated settings: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving settings: {ex.Message}");
            }
        }

        private static AppSettings Load()
        {
            var settings = new AppSettings();

            try
            {
                // Try to load from JSON file first
                if (File.Exists(SettingsPath))
                {
                    string json = File.ReadAllText(SettingsPath);
                    var loadedSettings = JsonSerializer.Deserialize<AppSettings>(json);
                    if (loadedSettings != null)
                    {
                        settings.DbServer = loadedSettings.DbServer;
                        settings.DbName = loadedSettings.DbName;
                        settings.DbUsername = loadedSettings.DbUsername;
                        settings.DbPassword = loadedSettings.DbPassword;
                        settings.DbPort = loadedSettings.DbPort;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading settings from JSON: {ex.Message}");
            }

            return settings;
        }
    }
}