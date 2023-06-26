using System.Windows.Input;
using System.Windows.Navigation;
using SystemMonitoringNetCore.Infrastructure.Command;
using SystemMonitoringNetCore.Models;
using SystemMonitoringNetCore.ViewModels.Base;
using SystemMonitoringNetCore.Views.Pages;
using SystemMonitoringNetCore.Views.UserControls;

namespace SystemMonitoringNetCore.ViewModels;

public class MainWindowViewModel : BaseViewModel
{
    #region Properties
    
    

    #endregion Properties
    
    #region Commands

    #region MenuMain - Перемещение на главную страницу

    /// <summary> Перемещение на главную страницу </summary>
    public ICommand MenuMainCommand { get; }

    private void OnMenuMainCommandExecuted(object parameter)
    {
        ManagerPage.ClearHistory();
        ManagerPage.Navigate(new MenuControl());
        ManagerPage.Page.BackStack.GetEnumerator().Reset();
    }

    private bool CanMenuMainCommandExecute(object parameter)
    {
        return true;
    }

    #endregion MenuMain

    #region MenuBack - Перемещение на прошлую страницу

    /// <summary> Перемещение на прошлую страницу </summary>
    public ICommand MenuBackCommand { get; }

    private void OnMenuBackCommandExecuted(object parameter)
    {
        ManagerPage.Back();
    }

    private bool CanMenuBackCommandExecute(object parameter) => ManagerPage.CanGoBack();

    #endregion MenuBack

    #region MenuFieldSelect - Перемещение на страницу выбора поля

    /// <summary> Перемещение на страницу выбора поля </summary>
    public ICommand MenuFieldSelectCommand { get; }

    private void OnMenuFieldSelectCommandExecuted(object parameter)
    {
        ManagerPage.ClearHistory();
        ManagerPage.Navigate(new FieldInfoControl());
    }

    private bool CanMenuFieldSelectCommandExecute(object parameter)
    {
        return true;
    }

    #endregion MenuFieldSelect

    #region MenuDocuments - Перемещение на страницу документов

    /// <summary> Перемещение на страницу документов </summary>
    public ICommand MenuDocumentsCommand { get; }

    private void OnMenuDocumentsCommandExecuted(object parameter)
    {
        ManagerPage.ClearHistory();
        ManagerPage.Navigate(new ReportsView());
    }

    private bool CanMenuDocumentsCommandExecute(object parameter)
    {
        return true;
    }

    #endregion MenuDocuments

    #region MenuCultures - Перемещение на страницу культур

    /// <summary> Перемещение на страницу культур </summary>
    public ICommand MenuCulturesCommand { get; }

    private void OnMenuCulturesCommandExecuted(object parameter)
    {
        ManagerPage.ClearHistory();
        ManagerPage.Navigate(new CulturesControl());
    }

    private bool CanMenuCulturesCommandExecute(object parameter)
    {
        return true;
    }

    #endregion MenuCultures

    #region MenuFertilizers - Перемещение на страницу удобрений

    /// <summary> Перемещение на страницу удобрений </summary>
    public ICommand MenuFertilizersCommand { get; }

    private void OnMenuFertilizersCommandExecuted(object parameter)
    {
        ManagerPage.ClearHistory();
        ManagerPage.Navigate(new FertilizersPage());
    }

    private bool CanMenuFertilizersCommandExecute(object parameter)
    {
        return true;
    }

    #endregion MenuFertilizers

    #region MenuExit - Перемещение на страницу авторизации

    /// <summary> Перемещение на страницу авторизации </summary>
    public ICommand MenuExitCommand { get; }

    private void OnMenuExitCommandExecuted(object parameter)
    {
        ManagerPage.ClearHistory();
        ManagerPage.Navigate(new AuthControl());
    }

    private bool CanMenuExitCommandExecute(object parameter)
    {
        return true;
    }

    #endregion MenuExit

    #region MenuSettings - Перемещение на страницу настроек

    ///<summary> Перемещение на страницу настроек </summary>
    public ICommand MenuSettingsCommand { get; }

    private void OnMenuSettingsCommandExecuted(object parameter)
    {
        var control = new SettingsControl();
        ManagerPage.Navigate(control);
    }

    private bool CanMenuSettingsCommandExecute(object parameter)
    {
        return true;
    }

    #endregion MenuSettings
    
    #endregion Commands

    #region Constructor

    public MainWindowViewModel()
    {
        MenuMainCommand = new RelayCommand(OnMenuMainCommandExecuted, CanMenuMainCommandExecute);
        MenuBackCommand = new RelayCommand(OnMenuBackCommandExecuted, CanMenuBackCommandExecute);
        MenuFieldSelectCommand = new RelayCommand(OnMenuFieldSelectCommandExecuted, CanMenuFieldSelectCommandExecute);
        MenuDocumentsCommand = new RelayCommand(OnMenuDocumentsCommandExecuted, CanMenuDocumentsCommandExecute);
        MenuCulturesCommand = new RelayCommand(OnMenuCulturesCommandExecuted, CanMenuCulturesCommandExecute);
        MenuFertilizersCommand = new RelayCommand(OnMenuFertilizersCommandExecuted, CanMenuFertilizersCommandExecute);
        MenuExitCommand = new RelayCommand(OnMenuExitCommandExecuted, CanMenuExitCommandExecute);
        MenuSettingsCommand = new RelayCommand(OnMenuSettingsCommandExecuted, CanMenuSettingsCommandExecute);
    }

    #endregion Constructor

    #region Private Methods

    

    #endregion Private Methods
}