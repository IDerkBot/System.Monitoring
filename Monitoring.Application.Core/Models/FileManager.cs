using System;
using System.IO;
using Newtonsoft.Json;

namespace SystemMonitoringNetCore.Models;

/// <summary>
/// Класс работы с файлами
/// </summary>
public static class FileManager
{
    /// <summary> Получение пути к файлу конфигурации </summary>
    /// <returns></returns>
    public static string GetPathConfig()
    {
        var appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var path = $@"{appdataPath}\{Db.DirectoryName}\{Db.ConfigName}.{Db.ConfigFormat}";
        if (File.Exists(path)) return path;

        if (!Directory.Exists($@"{appdataPath}\{Db.DirectoryName}\"))
            Directory.CreateDirectory($@"{appdataPath}\{Db.DirectoryName}\");
        File.Create(path).Dispose();
        var settings = new Settings();
        File.WriteAllText(path, JsonConvert.SerializeObject(settings));
        return path;
    }

    /// <summary>
    /// Получение данных настройки
    /// </summary>
    /// <returns></returns>
    public static Settings GetSettings()
    {
        return JsonConvert.DeserializeObject<Settings>(File.ReadAllText(GetPathConfig()));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="settings"></param>
    public static void SetSettings(Settings settings)
    {
        File.WriteAllText(GetPathConfig(), JsonConvert.SerializeObject(settings));
    }

    /// <summary>
    /// Получение папки AppData
    /// </summary>
    /// <returns>Путь к директории AppData</returns>
    public static string GetAppData()
    {
        var appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var path = $@"{appdataPath}\{Db.DirectoryName}\";
        if (!Directory.Exists(path))
            Directory.CreateDirectory($@"{appdataPath}\{Db.DirectoryName}\");
        return path;
    }

    /// <summary>
    /// Получение сенсоров из json
    /// </summary>
    /// <returns></returns>
    public static string GetSensorsJson()
    {
        var path = $@"{GetAppData()}\sensors.json";
        if (!File.Exists(path))
            File.Create(path).Dispose();
        return path;
    }
}