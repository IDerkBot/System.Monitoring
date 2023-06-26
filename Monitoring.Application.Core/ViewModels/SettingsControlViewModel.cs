using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
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

    public SettingsControlViewModel()
    {
        Settings = FileManager.GetSettings();
        ComPorts = SerialPort.GetPortNames().ToList();
    }
}