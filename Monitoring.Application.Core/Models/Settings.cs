using System;
using System.IO;
using Newtonsoft.Json;

namespace SystemMonitoringNetCore.Models;

public class Settings
{
    /// <summary>
    /// Логин
    /// </summary>
    public string Login { get; set; }

    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Запомнить данные
    /// </summary>
    public bool Remember { get; set; } = false;

    /// <summary>
    /// Путь к файлам
    /// </summary>
    public string ReportsPath { get; set; }

    public bool Save(string path)
    {
        try
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(this));
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool Load(string path)
    {
        try
        {
            JsonConvert.DeserializeObject<Settings>(File.ReadAllText(path));
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}