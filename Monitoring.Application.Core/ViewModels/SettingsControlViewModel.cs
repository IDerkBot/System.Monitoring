using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Windows.Input;
using SystemMonitoringNetCore.Infrastructure.Command;
using SystemMonitoringNetCore.Models;
using SystemMonitoringNetCore.ViewModels.Base;

namespace SystemMonitoringNetCore.ViewModels;

public class SettingsControlViewModel : BaseViewModel
{
    #region Settings : Settings - Настройки

    private Settings _settings;

    /// <summary> Настройки </summary>
    public Settings Settings
    {
        get => _settings;
        set => SetField(ref _settings, value);
    }

    #endregion Settings

    #region ComPorts : List<string> - COM порты

    private List<string> _comPorts;

    /// <summary> COM порты </summary>
    public List<string> ComPorts
    {
        get => _comPorts;
        set => SetField(ref _comPorts, value);
    }

    #endregion ComPorts

    #region Save - Сохранение данных

    /// <summary> Сохранение данных </summary>
    public ICommand SaveCommand { get; }

    private void OnSaveCommandExecuted(object parameter)
    {
        FileManager.SetSettings(Settings);
        ManagerPage.Back();
    }

    private bool CanSaveCommandExecute(object parameter)
    {
        return true;
    }

    #endregion Save
    
    public SettingsControlViewModel()
    {
        SaveCommand = new RelayCommand(OnSaveCommandExecuted, CanSaveCommandExecute);
        Settings = FileManager.GetSettings();
        ComPorts = SerialPort.GetPortNames().ToList();
    }
}