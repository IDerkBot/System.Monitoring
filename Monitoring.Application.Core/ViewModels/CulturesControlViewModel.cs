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

    #region Cultures : ObservableCollection<CultureStatus> - Список культур

    private ObservableCollection<CultureStatus> _cultures;

    /// <summary> Список культур </summary>
    public ObservableCollection<CultureStatus> Cultures
    {
        get => _cultures;
        set => SetField(ref _cultures, value);
    }

    #endregion Cultures

    #region SelectedCulture : CultureStatus - Выбранный статус культуры

    private CultureStatus _selectedCultureStatus;

    /// <summary> Выбранный статус культуры </summary>
    public CultureStatus SelectedCultureStatus
    {
        get => _selectedCultureStatus;
        set => SetField(ref _selectedCultureStatus, value);
    }

    #endregion SelectedCulture

    #endregion

    #region Loaded - Загрузка окна

    /// <summary> Загрузка окна </summary>
    public ICommand LoadedCommand { get; }

    private void OnLoadedCommandExecuted(object parameter)
    {
        UpdateCollection();
    }

    #endregion Loaded

    #region Add - Добавляет новый статус культуры

    /// <summary> Добавляет новый статус культуры </summary>
    public ICommand AddCommand { get; }

    private void OnAddCommandExecuted(object parameter) => ManagerPage.Navigate(new AddCultureStatusControlViewModel());

    #endregion Add

    #region Delete - Удаление статуса культуры

    /// <summary> Удаление статуса культуры </summary>
    public ICommand DeleteCommand { get; }

    private void OnDeleteCommandExecuted(object parameter)
    {
        if (parameter is CultureStatus cultureStatus)
        {
            Db.DbContext.CultureStatuses.Remove(cultureStatus);
            Db.DbContext.SaveChanges();
            
            UpdateCollection();
        }
    }

    private bool CanDeleteCommandExecute(object parameter) => parameter is CultureStatus;

    #endregion Delete

    #region Edit - Редактирование статуса культуры

    /// <summary> Редактирование статуса культуры </summary>
    public ICommand EditCommand { get; }

    private void OnEditCommandExecuted(object parameter)
    {
        if (parameter is not CultureStatus cultureStatus) return;
        var vm = new AddCultureStatusControlViewModel(cultureStatus);
        ManagerPage.Navigate(new AddCultureStatusControl { DataContext = vm });
    }

    private bool CanEditCommandExecute(object parameter) => parameter is CultureStatus;

    #endregion Edit

    #region Constructor

    public CulturesControlViewModel()
    {
        LoadedCommand = new RelayCommand(OnLoadedCommandExecuted);
        AddCommand = new RelayCommand(OnAddCommandExecuted);
        DeleteCommand = new RelayCommand(OnDeleteCommandExecuted, CanDeleteCommandExecute);
        EditCommand = new RelayCommand(OnEditCommandExecuted, CanEditCommandExecute);
        
        Cultures = new ObservableCollection<CultureStatus>();
    }

    #endregion

    #region Private Methods

    private void UpdateCollection()
    {
        Cultures.Clear();
        foreach (var culture in Db.DbContext.CultureStatuses.ToList()) 
            Cultures.Add(culture);
    }

    #endregion
}