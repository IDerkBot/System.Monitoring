using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Monitoring.Models.Entity;
using Newtonsoft.Json;
using SystemMonitoringNetCore.Infrastructure.Command;
using SystemMonitoringNetCore.Models;
using SystemMonitoringNetCore.ViewModels.Base;
using SystemMonitoringNetCore.Views.Pages;
using Sensor = SystemMonitoringNetCore.Models.Sensor;

namespace SystemMonitoringNetCore.ViewModels;

public class FieldInfoViewModel : BaseViewModel
{
    #region Переменные

    #region SelectedSeed : Seed - Выбранный посев

    private Seed _selectedSeed;

    /// <summary> Выбранный посев </summary>
    public Seed SelectedSeed
    {
        get => _selectedSeed;
        set => SetField(ref _selectedSeed, value);
    }

    #endregion SelectedSeed

    #region Cultures : ObservableCollection<Culture> - Список культур

    private ObservableCollection<Culture> _cultures;

    /// <summary> Список культур </summary>
    public ObservableCollection<Culture> Cultures
    {
        get => _cultures;
        set => SetField(ref _cultures, value);
    }

    #endregion Cultures

    #region SelectedCulture : Culture - Выбранная культура для поля

    private Culture _selectedCulture;

    /// <summary> Выбранная культура для поля </summary>
    public Culture SelectedCulture
    {
        get => _selectedCulture;
        set
        {
            SetField(ref _selectedCulture, value);
            SelectedSeed.Culture = _selectedCulture;
            Periods = Db.DbContext.CultureStatuses.Where(x => x.Culture.Id == SelectedSeed.Culture.Id).ToList();
            OnPropertyChanged(nameof(SelectedSeed.Culture));
        }
    }

    #endregion SelectedCulture

    #region Sensors : List<Sensor> - Список сенсоров

    private List<Sensor> _sensors;

    /// <summary> Список сенсоров </summary>
    public List<Sensor> Sensors
    {
        get => _sensors;
        set => SetField(ref _sensors, value);
    }

    #endregion Sensors

    #region GroundTypes : ObservableCollection<TypeGround> - Типы почв

    private ObservableCollection<TypeGround> _groundTypes;

    /// <summary> Типы почв </summary>
    public ObservableCollection<TypeGround> GroundTypes
    {
        get => _groundTypes;
        set => SetField(ref _groundTypes, value);
    }

    #endregion ObservableCollection<TypeGround>

    #region IsEditMode : bool - Включен режим редактирования

    private bool _isEditMode;

    /// <summary> Включен режим редактирования </summary>
    public bool IsEditMode
    {
        get => _isEditMode;
        set => SetField(ref _isEditMode, value);
    }

    #endregion IsEditMode

    #region Periods : List<CultureStatus> - Список статусов культур

    private List<CultureStatus> _periods;

    /// <summary> Список статусов культур </summary>
    public List<CultureStatus> Periods
    {
        get => _periods;
        set => SetField(ref _periods, value);
    }

    #endregion Periods

    #region SelectedPeriod : CultureStatus - Выбранный статус культуры

    private CultureStatus _selectedPeriod;

    /// <summary> Выбранный статус культуры </summary>
    public CultureStatus SelectedPeriod
    {
        get => _selectedPeriod;
        set
        {
            SetField(ref _selectedPeriod, value);
            SelectedSeed.Status = _selectedPeriod;
        }
    }

    #endregion SelectedPeriod

    #region TypeSoils : ObservableCollection<string> - Список типов почв

    private ObservableCollection<string> _typeSoils;

    /// <summary> Список типов почв </summary>
    public ObservableCollection<string> TypeSoils
    {
        get => _typeSoils;
        set => SetField(ref _typeSoils, value);
    }

    #endregion TypeSoils

    #endregion

    #region Команды

    #region Loaded - Загрузка окна

    /// <summary> Загрузка окна </summary>
    public ICommand LoadedCommand { get; }

    private void OnLoadedCommandExecuted(object parameter)
    {
        SelectedSeed = Db.SelectSeeding;
        Sensors = new List<Sensor>();

        SelectedCulture = SelectedSeed.Culture;
        SelectedPeriod = SelectedSeed.Status;
    }

    #endregion Loaded
    
    #region UpdateSensors - Обновление списка программ

    /// <summary> Обновление списка программ </summary>
    public ICommand UpdateSensorsCommand { get; }

    private void OnUpdateSensorsCommandExecute(object parameter)
    {
        GetTestSensors();
    }

    private bool CanUpdateSensorsCommandExecuted(object parameter)
    {
        return true;
    }

    #endregion UpdateSensors

    #region EditMode - Включает режим редактирования

    /// <summary> Включает режим редактирования </summary>
    public ICommand EditModeCommand { get; }

    private void OnEditModeCommandExecute(object parameter) => IsEditMode = true;

    private bool CanEditModeCommandExecuted(object parameter)
    {
        return true;
    }

    #endregion EditMode

    #region SaveData - Выключает режим редактирования и сохраняет данные

    /// <summary> Выключает режим редактирования и сохраняет данные </summary>
    public ICommand SaveDataCommand { get; }

    private void OnSaveDataCommandExecute(object parameter)
    {
        IsEditMode = false;
        Db.DbContext.SaveChanges();
    }

    private bool CanSaveDataCommandExecuted(object parameter)
    {
        return true;
    }

    #endregion SaveData

    #region OpenMap - Открывает карту поля

    /// <summary> Открывает карту поля </summary>
    public ICommand OpenMapCommand { get; }

    private void OnOpenMapCommandExecute(object parameter)
    {
    }

    private bool CanOpenMapCommandExecuted(object parameter)
    {
        return true;
    }

    #endregion OpenMap

    #region SaveSensorData - Сохраняет данные датчиков

    /// <summary> Сохраняет данные датчиков </summary>
    public ICommand SaveSensorDataCommand { get; }

    private void OnSaveSensorDataCommandExecute(object parameter)
    {
        if (Sensors.Count > 0)
        {
            List<SensorData> allData = new List<SensorData>();


            using (var fs = File.Open(Constants.SensorsData, FileMode.OpenOrCreate))
            {
                using (var sr = new StreamReader(fs))
                {
                    var str = sr.ReadToEnd();
                    var loadData = JsonConvert.DeserializeObject<List<SensorData>>(str);
                    if(loadData?.Count > 0) allData.AddRange(loadData);
                }
            }

            // var random = new Random();
            // foreach (var sensorData in allData)
            // {
            //     sensorData.Info.Temperature = random.Next(10, 15);
            // }
            
            using (var fs = File.Open(Constants.SensorsData, FileMode.OpenOrCreate))
            {
                allData.AddRange(Sensors.Select(x => new SensorData(x)));
                using (var sw = new StreamWriter(fs))
                {
                    sw.Write(JsonConvert.SerializeObject(allData));
                }
            }
        }
    }

    private bool CanSaveSensorDataCommandExecuted(object parameter)
    {
        return true;
    }

    #endregion SaveSensorData

    #region CreateExcelFileAboutSensor - Создает Excel файл с информацией об одном сенсоре

    /// <summary> Создает Excel файл с информацией об одном сенсоре </summary>
    public ICommand CreateExcelFileAboutSensorCommand { get; }

    private void OnCreateExcelFileAboutSensorCommandExecute(object parameter)
    {
    }

    private bool CanCreateExcelFileAboutSensorCommandExecuted(object parameter)
    {
        return true;
    }

    #endregion CreateExcelFileAboutSensor

    #region CreateExcelFileAboutAllSensor - Создает Excel файл с информацией обо всех сенсорах

    /// <summary> Создает Excel файл с информацией обо всех сенсорах </summary>
    public ICommand CreateExcelFileAboutAllSensorCommand { get; }

    private void OnCreateExcelFileAboutAllSensorCommandExecute(object parameter)
    {
    }

    private bool CanCreateExcelFileAboutAllSensorCommandExecuted(object parameter)
    {
        return true;
    }

    #endregion CreateExcelFileAboutAllSensor

    #region OpenChart - Открывает диаграммы по датчику

    /// <summary> Открывает диаграммы по датчику </summary>
    public ICommand OpenChartCommand { get; }

    private void OnOpenChartCommandExecute(object parameter)
    {
        var vm = new ChartControlViewModel(Sensors);
        ManagerPage.Navigate(new ChartControl {DataContext = vm});
    }

    private bool CanOpenChartCommandExecuted(object parameter) => Sensors is { Count: > 0 };

    #endregion OpenChart

    #region OpenChartAll - Открывает диаграммы по всем датчикам

    /// <summary> Открывает диаграммы по всем датчикам </summary>
    public ICommand OpenChartAllCommand { get; }

    private void OnOpenChartAllCommandExecute(object parameter)
    {
        ManagerPage.Navigate(new ChartsPage(Sensors));
    }

    private bool CanOpenChartAllCommandExecuted(object parameter) => Sensors is { Count: > 0 };

    #endregion OpenChartAll

    #endregion
    
    public FieldInfoViewModel()
    {
        #region Команды

        LoadedCommand = new RelayCommand(OnLoadedCommandExecuted);
        UpdateSensorsCommand = new RelayCommand(OnUpdateSensorsCommandExecute, CanUpdateSensorsCommandExecuted);
        EditModeCommand = new RelayCommand(OnEditModeCommandExecute, CanEditModeCommandExecuted);
        SaveDataCommand = new RelayCommand(OnSaveDataCommandExecute, CanSaveDataCommandExecuted);
        OpenMapCommand = new RelayCommand(OnOpenChartCommandExecute, CanOpenChartCommandExecuted);
        SaveSensorDataCommand = new RelayCommand(OnSaveSensorDataCommandExecute, CanSaveSensorDataCommandExecuted);
        CreateExcelFileAboutSensorCommand = new RelayCommand(OnCreateExcelFileAboutSensorCommandExecute,
            CanCreateExcelFileAboutSensorCommandExecuted);
        CreateExcelFileAboutAllSensorCommand = new RelayCommand(OnCreateExcelFileAboutAllSensorCommandExecute,
            CanCreateExcelFileAboutAllSensorCommandExecuted);
        OpenChartCommand = new RelayCommand(OnOpenChartCommandExecute, CanOpenChartCommandExecuted);
        OpenChartAllCommand = new RelayCommand(OnOpenChartAllCommandExecute, CanOpenChartAllCommandExecuted);

        #endregion

        Cultures = new ObservableCollection<Culture>(Db.DbContext.Cultures.ToList());
        GroundTypes = new ObservableCollection<TypeGround>(Db.DbContext.TypeGrounds.ToList());

        TypeSoils = new ObservableCollection<string>()
        {
            "Чернозем", "Тундровые", "Подзолистые", "Болотные", "Серые лесные", "Луговые"
        };
    }

    private void GetTestSensors()
    {
        var rand = new Random();
        Sensors = new List<Sensor>();

        for (var i = 1; i <= 10; i++)
        {
            Sensors.Add(new Sensor
            {
                Id = i, Acidity = rand.Next(0, 100), Sodium = rand.Next(0, 100), Salinity = rand.Next(0, 100),
                Humidity = rand.Next(0, 100), Potassium = rand.Next(0, 100),
                Phosphorus = rand.Next(0, 100), Temperature = rand.Next(10, 20), Recommendation = $"Тестовый датчик {i}"
            });
        }
    }
}