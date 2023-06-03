using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Monitoring.Models.Entity;
using SystemMonitoringNetCore.Infrastructure.Command;
using SystemMonitoringNetCore.Models;
using SystemMonitoringNetCore.ViewModels.Base;
using SystemMonitoringNetCore.Views.UserControls;

namespace SystemMonitoringNetCore.ViewModels;

public class CulturesControlViewModel : BaseViewModel
{
    #region Properties

    #region Cultures : ObservableCollection<Culture> - Список культур

    private ObservableCollection<Culture> _cultures;

    /// <summary> Список культур </summary>
    public ObservableCollection<Culture> Cultures
    {
        get => _cultures;
        set => SetField(ref _cultures, value);
    }

    #endregion Cultures

    #region SelectedCulture : Culture - Выбранная культура

    private Culture _selectedCulture;

    /// <summary> Выбранный статус культуры </summary>
    public Culture SelectedCulture
    {
        get => _selectedCulture;
        set => SetField(ref _selectedCulture, value);
    }

    #endregion SelectedCulture

    #endregion

    #region Commands

    #region Loaded - Загрузка окна

    /// <summary> Загрузка окна </summary>
    public ICommand LoadedCommand { get; }

    private void OnLoadedCommandExecuted(object parameter)
    {
        UpdateCollection();
    }

    #endregion Loaded

    #region SelectCulture - Выбор культуры и открытие информации о нем

    /// <summary> Выбор культуры и открытие информации о нем </summary>
    public ICommand SelectCultureCommand { get; }

    private void OnSelectCultureCommandExecuted(object parameter)
    {
        if (parameter is Culture culture)
        {
            var vm = new CultureInfoControlViewModel(culture);
            ManagerPage.Navigate(new CultureInfoControl { DataContext = vm });
        }
    }

    #endregion SelectCulture

    #region AddNewCulture - Добавление новой культуры

    /// <summary> Добавление новой культуры </summary>
    public ICommand AddNewCultureCommand { get; }

    private void OnAddNewCultureCommandExecuted(object parameter)
    {
        var vm = new CultureInfoControlViewModel(new Culture());
        ManagerPage.Navigate(new CultureInfoControl { DataContext = vm });
    }

    #endregion AddNewCulture

    #region DeleteSelectedCulture - Удаление выбранной культуры

    /// <summary> Удаление выбранной культуры </summary>
    public ICommand DeleteSelectedCultureCommand { get; }

    private void OnDeleteSelectedCultureCommandExecuted(object parameter)
    {
        if (parameter is Culture culture)
        {
            Db.DbContext.Cultures.Remove(culture);
            Db.DbContext.SaveChanges();
            UpdateCollection();
        }
    }

    private bool CanDeleteSelectedCultureCommandExecute(object parameter) => parameter is Culture;

    #endregion DeleteSelectedCulture

    #endregion
    
    #region Constructor

    public CulturesControlViewModel()
    {
        LoadedCommand = new RelayCommand(OnLoadedCommandExecuted);
        SelectCultureCommand = new RelayCommand(OnSelectCultureCommandExecuted);
        AddNewCultureCommand = new RelayCommand(OnAddNewCultureCommandExecuted);
        DeleteSelectedCultureCommand = new RelayCommand(OnDeleteSelectedCultureCommandExecuted, CanDeleteSelectedCultureCommandExecute);
        
        Cultures = new ObservableCollection<Culture>();
    }

    #endregion

    #region Private Methods

    private void UpdateCollection()
    {
        Cultures.Clear();
        foreach (var culture in Db.DbContext.Cultures.ToList()) 
            Cultures.Add(culture);
    }

    #endregion
}