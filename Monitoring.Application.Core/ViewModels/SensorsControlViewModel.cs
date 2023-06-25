using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Monitoring.Models.Entity;
using SystemMonitoringNetCore.Infrastructure.Command;
using SystemMonitoringNetCore.Models;
using SystemMonitoringNetCore.ViewModels.Base;
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
        var vm = new EditSensorControlViewModel();
        ManagerPage.Navigate(new SensorAddControl() {DataContext = vm});
    }

    private bool CanAddSensorCommandExecute(object parameter)
    {
        return true;
    }

    #endregion AddSensor

    #region RemoveSensor - Удалить датчик

    /// <summary> Удалить датчик </summary>
    public ICommand RemoveSensorCommand { get; }

    private void OnRemoveSensorCommandExecuted(object parameter)
    {

    }

    private bool CanRemoveSensorCommandExecute(object parameter)
    {
        return true;
    }

    #endregion RemoveSensor

    #region EditSensor - Редактировать датчик

    /// <summary> Редактировать датчик </summary>
    public ICommand EditSensorCommand { get; }

    private void OnEditSensorCommandExecuted(object parameter)
    {

    }

    private bool CanEditSensorCommandExecute(object parameter)
    {
        return true;
    }

    #endregion EditSensor

    #endregion Commands

    #region Constructor

    public SensorsControlViewModel()
    {
        LoadedCommand = new RelayCommand(OnLoadedCommandExecuted, CanLoadedCommandExecute);
        AddSensorCommand = new RelayCommand(OnAddSensorCommandExecuted, CanAddSensorCommandExecute);
        RemoveSensorCommand = new RelayCommand(OnRemoveSensorCommandExecuted, CanRemoveSensorCommandExecute);
        EditSensorCommand = new RelayCommand(OnEditSensorCommandExecuted, CanEditSensorCommandExecute);
    }

    #endregion Constructor
}