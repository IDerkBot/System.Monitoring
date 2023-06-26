using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using Monitoring.Models.Entity;
using SystemMonitoringNetCore.Infrastructure.Command;
using SystemMonitoringNetCore.Models;
using SystemMonitoringNetCore.ViewModels.Base;
using SystemMonitoringNetCore.Views.Windows;

namespace SystemMonitoringNetCore.ViewModels;

public class LoadSensorsDialogViewModel : BaseViewModel
{
    #region Properties

    #region LoadedSensors : uint - Количество загруженных датчиков

    private int _loadedSensors;

    /// <summary> Количество загруженных датчиков </summary>
    public int LoadedSensors
    {
        get => _loadedSensors;
        set => SetField(ref _loadedSensors, value);
    }

    #endregion LoadedSensors

    #region MaxSensors : uint - Максимальное количество датчиков

    private int _maxSensors;

    /// <summary> Максимальное количество датчиков </summary>
    public int MaxSensors
    {
        get => _maxSensors;
        set => SetField(ref _maxSensors, value);
    }

    #endregion MaxSensors

    #region LoadSensor : string - Загружаемый датчик

    private string _loadSensor;

    /// <summary> Загружаемый датчик </summary>
    public string LoadSensor
    {
        get => _loadSensor;
        set => SetField(ref _loadSensor, value);
    }

    #endregion LoadSensor

    #endregion Properties

    #region Private Properties

    private static readonly Timer Tm = new(10000);
    public event EventHandler Loaded;
    private Window _dialog;
    private Field _field;
    public List<SensorData> SensorData;

    #endregion Private Properties

    private int _index;

    public LoadSensorsDialogViewModel(Field field)
    {
        _field = field;
        LoadedCommand = new RelayCommand(OnLoadedCommandExecuted, CanLoadedCommandExecute);
        _dialog = new LoadSensorsDialog { DataContext = this };
        _dialog.Show();
    }

    #region Loaded - Description

    /// <summary> Description </summary>
    public ICommand LoadedCommand { get; }

    private void OnLoadedCommandExecuted(object parameter)
    {
        GetSensors();
    }

    private bool CanLoadedCommandExecute(object parameter)
    {
        return true;
    }

    #endregion Loaded

    private ArduinoWorker _worker;
    
    private void GetSensors()
    {
        _worker = new ArduinoWorker("COM9");
        _worker.Load += WorkerOnLoad;
        _worker.Complete += WorkerOnComplete;

        var sensors = Db.DbContext.Sensors.Where(x => x.Field.Number == _field.Number);
        MaxSensors = sensors.Count();
        if(!sensors.Any()) _dialog.Close();
        else
        {
            _worker.Connect();
            _worker.ReadSensors(sensors.Select(x => x.Uid.ToString()).ToArray());
        }
    }

    private void WorkerOnLoad(object? sender, EventArgs e)
    {
        LoadedSensors = _worker.Index;
    }
    
    private void WorkerOnComplete(object? sender, EventArgs e)
    {
        SensorData = _worker.SensorData;
        _dialog.Dispatcher.Invoke(() => _dialog.Close());
        Loaded?.Invoke(this, EventArgs.Empty);
    }
}