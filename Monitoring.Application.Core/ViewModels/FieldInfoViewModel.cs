using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using Monitoring.Models.Entity;
using Newtonsoft.Json;
using SystemMonitoringNetCore.Infrastructure.Command;
using SystemMonitoringNetCore.Models;
using SystemMonitoringNetCore.ViewModels.Base;
using SystemMonitoringNetCore.Views.Pages;
using Sensor = Monitoring.Models.Entity.Sensor;

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
        set
        {
            SetField(ref _selectedSeed, value);
            SelectedCulture = SelectedSeed.Culture;
            SelectedTypeGround = SelectedSeed.TypeGround;
            SelectedPeriod = SelectedSeed.Status;
        }
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

    #region SelectedCulture : Culture? - Выбранная культура для поля

    private Culture? _selectedCulture;

    /// <summary> Выбранная культура для поля </summary>
    public Culture? SelectedCulture
    {
        get => _selectedCulture;
        set
        {
            SetField(ref _selectedCulture, value);
            SelectedSeed.Culture = _selectedCulture;
            if (SelectedSeed.Culture != null)
                Periods = Db.DbContext.CultureStatuses.Where(x => x.Culture.Id == SelectedSeed.Culture.Id).ToList();
            OnPropertyChanged(nameof(SelectedSeed));
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

    #region SelectedPeriod : CultureStatus? - Выбранный статус культуры

    private CultureStatus? _selectedPeriod;

    /// <summary> Выбранный статус культуры </summary>
    public CultureStatus? SelectedPeriod
    {
        get => _selectedPeriod;
        set
        {
            SetField(ref _selectedPeriod, value);
            SelectedSeed.Status = _selectedPeriod;
            OnPropertyChanged(nameof(SelectedSeed));
        }
    }

    #endregion SelectedPeriod

    #region SelectedTypeGround : TypeGround? - Выбранный тип почвы

    private TypeGround? _selectedTypeGround;

    /// <summary> Выбранный тип почвы </summary>
    public TypeGround? SelectedTypeGround
    {
        get => _selectedTypeGround;
        set
        {
            SetField(ref _selectedTypeGround, value);
            SelectedSeed.TypeGround = SelectedTypeGround;
            OnPropertyChanged(nameof(SelectedSeed));
        }
    }

    #endregion SelectedTypeGround

    #region TypeSoils : ObservableCollection<string> - Список типов почв

    private ObservableCollection<TypeGround> _typeSoils;

    /// <summary> Список типов почв </summary>
    public ObservableCollection<TypeGround> TypeSoils
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

        AddSensorDataInDatabase();
    }

    private void AddSensorDataInDatabase()
    {
        Sensors.ForEach(x =>
        {
            var sensorData = new SensorData()
            {
                DateTime = DateTime.Now,
                Sensor = x,
                CultureStatus = SelectedSeed.Status != null ? SelectedSeed.Status.Status : "Не выбрано",
                Temperature = x.Temperature,
                Sodium = x.Sodium,
                Salinity = x.Salinity,
                Humidity = x.Humidity,
                Phosphorus = x.Phosphorus,
                Acidity = x.Acidity,
                Potassium = x.Potassium,
                Recommendation = x.Recommendation
            };

            Db.DbContext.SensorData.Add(sensorData);
            Db.DbContext.SaveChanges();
        });
    }

    private bool CanUpdateSensorsCommandExecuted(object parameter) => true;

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
        // if (Sensors.Count > 0)
        // {
        //     new Thread(() =>
        //     {
        //         List<SensorData> allData = new List<SensorData>();
        //
        //     var rand = new Random();
        //     for (int i = 1; i <= 10; i++)
        //     {
        //         var sensor = new Sensor
        //         {
        //             Id = 0, Acidity = rand.Next(0, 100), Sodium = rand.Next(0, 100), Salinity = rand.Next(0, 100),
        //             Humidity = rand.Next(0, 100), Potassium = rand.Next(0, 100),
        //             Phosphorus = rand.Next(0, 100), Temperature = rand.Next(10, 16), Recommendation = $"Тестовый датчик {i}"
        //         };
        //
        //         Db.DbContext.Sensors.Add(sensor);
        //         Db.DbContext.SaveChanges();
        //
        //         for (int j = 1; j < 366; j++)
        //         {
        //             sensor.Id = i;
        //             sensor.Sodium = rand.Next(0, 100);
        //             sensor.Salinity = rand.Next(0, 100);
        //             sensor.Humidity = rand.Next(0, 100);
        //             sensor.Potassium = rand.Next(0, 100);
        //             sensor.Phosphorus = rand.Next(0, 100);
        //             sensor.Temperature = j < 10 ? rand.Next(10, 16) : rand.Next(12, 15);
        //             sensor.Recommendation = $"Тестовый датчик {i}";
        //             var date = new DateTime(2023, 05, 01).AddDays(j);
        //             var data = new SensorData
        //             {
        //                 Sensor = sensor,
        //                 DateTime = date, 
        //                 CultureStatus = j < 20 ? "Засев" : j < 25 ? "Бутонизация" : j < 28 ? "начало цветения" : j < 60 ? "4 фаза" : "5 фаза",
        //                 Temperature = sensor.Temperature,
        //                 Sodium = sensor.Sodium,
        //                 Salinity = sensor.Salinity,
        //                 Humidity = sensor.Humidity,
        //                 Potassium = sensor.Potassium,
        //                 Phosphorus = sensor.Phosphorus,
        //                 Recommendation = sensor.Recommendation
        //             };
        //
        //             Db.DbContext.SensorData.Add(data);
        //             Db.DbContext.SaveChanges();
        //             // allData.Add(data);
        //         }
        //     }
        //     }).Start();

            // using (var fs = File.Open(Constants.SensorsData, FileMode.OpenOrCreate))
            // {
            //     using (var sw = new StreamWriter(fs))
            //     {
            //         sw.Write(JsonConvert.SerializeObject(allData));
            //     }
            // }

            return;

            // // Читаем данные из файла
            // using (var fs = File.Open(Constants.SensorsData, FileMode.OpenOrCreate))
            // {
            //     using (var sr = new StreamReader(fs))
            //     {
            //         var str = sr.ReadToEnd();
            //         var loadData = JsonConvert.DeserializeObject<List<SensorData>>(str);
            //         if (loadData?.Count > 0) allData.AddRange(loadData);
            //     }
            // }
            //
            // // Загружаем данные из сенсоров которые у нас есть
            // using (var fs = File.Open(Constants.SensorsData, FileMode.OpenOrCreate))
            // {
            //     allData.AddRange(Sensors.Select(x => new SensorData()));
            //     using (var sw = new StreamWriter(fs))
            //     {
            //         sw.Write(JsonConvert.SerializeObject(allData));
            //     }
            // }
        // }
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
        var vm = new ChartControlViewModel(Sensors, SelectedSeed.Status);
        ManagerPage.Navigate(new ChartControl { DataContext = vm });
    }

    private bool CanOpenChartCommandExecuted(object parameter) => Sensors is { Count: > 0 };

    #endregion OpenChart

    #region OpenChartAll - Открывает диаграммы по всем датчикам

    /// <summary> Открывает диаграммы по всем датчикам </summary>
    public ICommand OpenChartAllCommand { get; }

    private void OnOpenChartAllCommandExecute(object parameter)
    {
        var vm = new AllChartsControlViewModel(Sensors, SelectedSeed.Status);
        ManagerPage.Navigate(new AllChartsControl() { DataContext = vm });
        // ManagerPage.Navigate(new ChartsPage(Sensors));
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
        TypeSoils = new ObservableCollection<TypeGround>(Db.DbContext.TypeGrounds.ToList());
        // "Чернозем", "Тундровые", "Подзолистые", "Болотные", "Серые лесные", "Луговые"
    }

    private void GetTestSensors()
    {
        var rand = new Random();
        Sensors = new List<Sensor>();

        for (var i = 1; i <= 100; i++)
        {
            var sensor = new Sensor
            {
                Id = i, Acidity = rand.Next(0, 100), Sodium = rand.Next(0, 100), Salinity = rand.Next(0, 100),
                Humidity = rand.Next(0, 100), Potassium = rand.Next(0, 100),
                Phosphorus = rand.Next(0, 100), Temperature = rand.Next(10, 20), Recommendation = $"Тестовый датчик {i}"
            };
            Sensors.Add(sensor);
            // if (Db.DbContext.Sensors.All(x => x.Id != sensor.Id))
            //     Db.DbContext.Sensors.Add(sensor);
            
            Db.DbContext.SaveChanges();

            // var currentSensor = Db.DbContext.Sensors.First(x => x.Id == sensor.Id);
            
            // for (int j = 0; j < 365; j++)
            // {
            //     Db.DbContext.SensorData.Add(new SensorData { DateTime = new DateTime(2023, 01, 01).AddDays(j), SensorInfo = currentSensor, CultureStatus = "Засев" });
            //     Db.DbContext.SaveChanges();
            // }
        }

        OnPropertyChanged(nameof(Sensors));
    }
}