using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Monitoring.Models.Entity;
using SystemMonitoringNetCore.Infrastructure.Command;
using SystemMonitoringNetCore.Models;
using SystemMonitoringNetCore.ViewModels.Base;
using SystemMonitoringNetCore.Views.Pages;
using SystemMonitoringNetCore.Views.UserControls;

namespace SystemMonitoringNetCore.ViewModels;

public class SensorsControlViewModel : BaseViewModel
{
    #region Properties

    #region Sensors : ObservableCollection<Sensor> - Коллекция датчиков

    private ObservableCollection<Sensor> _sensors;

    /// <summary> Коллекция датчиков </summary>
    public ObservableCollection<Sensor> Sensors
    {
        get => _sensors;
        set => SetField(ref _sensors, value);
    }

    #endregion Sensors

    #region SelectedSensor : Sensor - Выбранный датчик

    private Sensor _selectedSensor;

    /// <summary> Выбранный датчик </summary>
    public Sensor SelectedSensor
    {
        get => _selectedSensor;
        set => SetField(ref _selectedSensor, value);
    }

    #endregion SelectedSensor
    
    #endregion Properties

    #region Commands

    #region Loaded - Загрузка

    /// <summary> Загрузка </summary>
    public ICommand LoadedCommand { get; }

    private void OnLoadedCommandExecuted(object parameter)
    {
        Sensors = new ObservableCollection<Sensor>(Db.DbContext.Sensors.ToList());
    }

    private bool CanLoadedCommandExecute(object parameter)
    {
        return true;
    }

    #endregion Loaded

    #region AddSensor - Добавить датчик

    /// <summary> Добавить датчик </summary>
    public ICommand AddSensorCommand { get; }

    private void OnAddSensorCommandExecuted(object parameter)
    {
        var vm = new SensorEditControlViewModel();
        ManagerPage.Navigate(new SensorEditControl {DataContext = vm});
    }

    private bool CanAddSensorCommandExecute(object parameter) => Db.CurrentUser.Access >= 2;

    #endregion AddSensor

    #region RemoveSensor - Удалить датчик

    /// <summary> Удалить датчик </summary>
    public ICommand RemoveSensorCommand { get; }

    private void OnRemoveSensorCommandExecuted(object parameter)
    {
        if (parameter is Sensor sensor)
        {
            Db.DbContext.Sensors.Remove(sensor);
        }
    }

    private bool CanRemoveSensorCommandExecute(object parameter) => parameter is Sensor && Db.CurrentUser.Access >= 2;

    #endregion RemoveSensor

    #region EditSensor - Редактировать датчик

    /// <summary> Редактировать датчик </summary>
    public ICommand EditSensorCommand { get; }

    private void OnEditSensorCommandExecuted(object parameter)
    {
        if (parameter is Sensor sensor)
        {
            var vm = new SensorEditControlViewModel(sensor);
            ManagerPage.Navigate(new SensorEditControl {DataContext = vm});
        }
    }

    private bool CanEditSensorCommandExecute(object parameter) => parameter is Sensor && Db.CurrentUser.Access >= 2;

    #endregion EditSensor

    #region OpenMap - Открыть карту

    ///<summary> Открыть карту </summary>
    public ICommand OpenMapCommand { get; }

    private void OnOpenMapCommandExecuted(object parameter)
    {
        var vm = new MapControlViewModel(Sensors.ToList());
        ManagerPage.Navigate(new MapPage { DataContext = vm });
    }

    private bool CanOpenMapCommandExecute(object parameter)
    {
        return true;
    }

    #endregion OpenMap
    
    #endregion Commands

    #region Constructor

    public SensorsControlViewModel()
    {
        LoadedCommand = new RelayCommand(OnLoadedCommandExecuted, CanLoadedCommandExecute);
        AddSensorCommand = new RelayCommand(OnAddSensorCommandExecuted, CanAddSensorCommandExecute);
        RemoveSensorCommand = new RelayCommand(OnRemoveSensorCommandExecuted, CanRemoveSensorCommandExecute);
        EditSensorCommand = new RelayCommand(OnEditSensorCommandExecuted, CanEditSensorCommandExecute);
        OpenMapCommand = new RelayCommand(OnOpenMapCommandExecuted, CanOpenMapCommandExecute);
    }

    #endregion Constructor
}