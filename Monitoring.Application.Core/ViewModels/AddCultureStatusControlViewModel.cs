using System.Collections.ObjectModel;
using System.Windows.Input;
using Monitoring.Models.Entity;
using SystemMonitoringNetCore.Infrastructure.Command;
using SystemMonitoringNetCore.Models;
using SystemMonitoringNetCore.ViewModels.Base;

namespace SystemMonitoringNetCore.ViewModels;

public class AddCultureStatusControlViewModel : BaseViewModel
{
    #region Properties

    #region CurrentCultureStatus : CultureStatus - Статус культуры

    private CultureStatus _currentCultureStatus;

    /// <summary> Статус культуры </summary>
    public CultureStatus CurrentCultureStatus
    {
        get => _currentCultureStatus;
        set => SetField(ref _currentCultureStatus, value);
    }

    #endregion CurrentCultureStatus

    #region ListCultures : ObservableCollection<Culture> - Список культур

    private ObservableCollection<Culture> _listCultures;

    /// <summary> Список культур </summary>
    public ObservableCollection<Culture> ListCultures
    {
        get => _listCultures;
        set => SetField(ref _listCultures, value);
    }

    #endregion ListCultures
    
    #endregion

    #region Commands

    #region Loaded - Загрузка

    /// <summary> Загрузка </summary>
    public ICommand LoadedCommand { get; }

    private void OnLoadedCommandExecuted(object parameter)
    {
        ListCultures.Clear();
        foreach (var culture in Db.DbContext.Cultures)
            ListCultures.Add(culture);
    }

    #endregion Loaded

    #region Save - Сохранение данных

    /// <summary> Сохранение данных </summary>
    public ICommand SaveCommand { get; }

    private void OnSaveCommandExecuted(object parameter)
    {
        if (CurrentCultureStatus.Id == 0) Db.DbContext.CultureStatuses.Add(CurrentCultureStatus);
        Db.DbContext.SaveChanges();
        ManagerPage.Back();
    }

    private bool CanSaveCommandExecute(object parameter) =>
        !string.IsNullOrWhiteSpace(CurrentCultureStatus.Status) &&
        CurrentCultureStatus.StartPeriod > 0 &&
        CurrentCultureStatus.EndPeriod > 0 &&
        CurrentCultureStatus.StartingValueCalcium > 0 &&
        CurrentCultureStatus.EndingValueCalcium > 0 &&
        CurrentCultureStatus.StartingValueTemperature > 0 &&
        CurrentCultureStatus.EndingValueTemperature > 0 &&
        CurrentCultureStatus.StartingValueHumidity > 0 &&
        CurrentCultureStatus.EndingValueHumidity > 0 &&
        CurrentCultureStatus.StartingValueMagnesium > 0 &&
        CurrentCultureStatus.EndingValueMagnesium > 0 &&
        CurrentCultureStatus.StartingValueNitrogen > 0 &&
        CurrentCultureStatus.EndingValueNitrogen > 0 &&
        CurrentCultureStatus.StartingValuePh > 0 &&
        CurrentCultureStatus.EndingValuePh > 0 &&
        CurrentCultureStatus.StartingValuePhosphor > 0 &&
        CurrentCultureStatus.EndingValuePhosphor > 0 &&
        CurrentCultureStatus.StartingValuePotassium > 0 &&
        CurrentCultureStatus.EndingValuePotassium > 0;
    

    #endregion Save

    #region AddNewCulture - Добавление новой культуры

    /// <summary> Добавление новой культуры </summary>
    public ICommand AddNewCultureCommand { get; }

    private void OnAddNewCultureCommandExecuted(object parameter)
    {
        ManagerPage.Navigate(new AddCultureControlViewModel());
    }

    #endregion AddNewCulture
    
    #endregion

    #region Contructor

    public AddCultureStatusControlViewModel(CultureStatus? cultureStatus = null)
    {
        CurrentCultureStatus = cultureStatus ?? new CultureStatus();
        LoadedCommand = new RelayCommand(OnLoadedCommandExecuted);
        SaveCommand = new RelayCommand(OnSaveCommandExecuted, CanSaveCommandExecute);
        AddNewCultureCommand = new RelayCommand(OnAddNewCultureCommandExecuted);
        ListCultures = new ObservableCollection<Culture>();
    }

    #endregion
}