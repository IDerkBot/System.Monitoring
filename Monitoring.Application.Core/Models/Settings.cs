using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace SystemMonitoringNetCore.Models;

public class Settings : ObservableObject
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

    #region ComPort : string - COM порт для подключение к базовой станции

    private string _comPort;

    /// <summary> COM порт для подключение к базовой станции </summary>
    public string ComPort
    {
        get => _comPort;
        set => SetProperty(ref _comPort, value);
    }

    #endregion ComPort

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