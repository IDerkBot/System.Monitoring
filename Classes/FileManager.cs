using Newtonsoft.Json;
using System;
using System.IO;
using SystemMonitoring.Models;

namespace SystemMonitoring
{
    class FileManager
    {
        public static string GetPathConfig()
        {
            string appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string path = $@"{appdataPath}\{DB.DirectoryName}\{DB.ConfigName}.{DB.ConfigFormat}";
            if (!File.Exists(path))
            {
                if (!Directory.Exists($@"{appdataPath}\{DB.DirectoryName}\")) Directory.CreateDirectory($@"{appdataPath}\{DB.DirectoryName}\");
                File.Create(path).Dispose();
                Settings settings = new Settings();
                File.WriteAllText(path, JsonConvert.SerializeObject(settings));
            }
            return path;
        }
        public static Settings GetSettings() { return JsonConvert.DeserializeObject<Settings>(File.ReadAllText(GetPathConfig())); }
        public static void SetSettings(Settings settings) { File.WriteAllText(GetPathConfig(), JsonConvert.SerializeObject(settings)); }
        public static string GetAppData()
        {
            string appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string path = $@"{appdataPath}\{DB.DirectoryName}\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory($@"{appdataPath}\{DB.DirectoryName}\");
            return path;
        }
        public static string GetSensorsJson()
        {
            string path = $@"{GetAppData()}\sensors.json";
            if (!File.Exists(path))
                File.Create(path).Dispose();
            return path;
        }
    }
}