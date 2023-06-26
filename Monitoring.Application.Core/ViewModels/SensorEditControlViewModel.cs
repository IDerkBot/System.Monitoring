using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Monitoring.Models.Entity;
using SystemMonitoringNetCore.Infrastructure.Command;
using SystemMonitoringNetCore.Models;
using SystemMonitoringNetCore.ViewModels.Base;

namespace SystemMonitoringNetCore.ViewModels;

public class SensorEditControlViewModel : BaseViewModel
{
    #region Properties

    #region Sensor : Sensor - Датчик

    private Sensor _sensor = new();

    /// <summary> Датчик </summary>
    public Sensor Sensor
    {
        get => _sensor;
        set => SetField(ref _sensor, value);
    }

    #endregion Sensor

    #region Fields : ObservableCollection<Field> - Коллекция полей

    private ObservableCollection<Field> _fields;

    /// <summary> Коллекция полей </summary>
    public ObservableCollection<Field> Fields
    {
        get => _fields;
        set => SetField(ref _fields, value);
    }

    #endregion Fields

    #endregion Properties
    
    #region Commands

    #region Save - Сохранение

    /// <summary> Сохранение </summary>
    public ICommand SaveCommand { get; }

    private void OnSaveCommandExecuted(object parameter)
    {
        if (!Db.DbContext.Sensors.Any(x => x.Uid == Sensor.Uid))
        {
            Db.DbContext.Sensors.Add(Sensor);
        }
        Db.DbContext.SaveChanges();
        ManagerPage.Back();
    }

    private bool CanSaveCommandExecute(object parameter)
    {
        return true;
    }

    #endregion Save

    #endregion Commands
    
    public SensorEditControlViewModel()
    {
        SaveCommand = new RelayCommand(OnSaveCommandExecuted, CanSaveCommandExecute);
        Fields = new ObservableCollection<Field>(Db.DbContext.Fields);
    }

    public SensorEditControlViewModel(Sensor sensor) : this()
    {
        Sensor = sensor;
    }
}