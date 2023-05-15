using System.Windows.Input;
using SystemMonitoringNetCore.Infrastructure.Command;
using SystemMonitoringNetCore.Models;
using SystemMonitoringNetCore.Views.Pages;
using SystemMonitoringNetCore.Views.UserControls;

namespace SystemMonitoringNetCore.ViewModels;

public class MenuViewModel
{
    #region MoveFields - Перемещение на страницу выбора поля

    /// <summary> Перемещение на страницу выбора поля </summary>
    public ICommand MoveFieldsCommand { get; }

    private void OnMoveFieldsCommandExecute(object parameter) => ManagerPage.Navigate(new FieldSelectPage());

    #endregion MoveFileds

    #region MoveDocuments - Перемещение на страницу документов

    /// <summary> Перемещение на страницу документов </summary>
    public ICommand MoveDocumentsCommand { get; }

    private void OnMoveDocumentsCommandExecute(object parameter) => ManagerPage.Navigate(new ReportsPage());

    #endregion MoveDocuments

    #region MoveCultures - Перемещение на страницу культур

    /// <summary> Перемещение на страницу культур </summary>
    public ICommand MoveCulturesCommand { get; }

    private void OnMoveCulturesCommandExecute(object parameter) => ManagerPage.Navigate(new CulturesPage());

    #endregion MoveCultures

    #region MoveFertilizers - Перемещение на страницу удобрений

    /// <summary> Перемещение на страницу удобрений </summary>
    public ICommand MoveFertilizersCommand { get; }

    private void OnMoveFertilizersCommandExecute(object parameter) => ManagerPage.Navigate(new FertilizersPage());

    #endregion MoveFertilizers
    
    #region MoveUsers - Перемещение на страницу пользователей

    /// <summary> Перемещение на страницу пользователей </summary>
    public ICommand MoveUsersCommand { get; }

    private static void OnMoveUsersCommandExecute(object parameter) => ManagerPage.Navigate(new UsersControl());

    private bool CanMoveUsersCommandExecuted(object parameter)
    {
        // TODO В зависимости от уровня доступа давать и не давать заходить на окно пользователей
        return true;
    }

    #endregion MoveUsers

    public MenuViewModel()
    {
        MoveFieldsCommand = new RelayCommand(OnMoveFieldsCommandExecute);
        MoveDocumentsCommand = new RelayCommand(OnMoveDocumentsCommandExecute);
        MoveCulturesCommand = new RelayCommand(OnMoveCulturesCommandExecute);
        MoveFertilizersCommand = new RelayCommand(OnMoveFertilizersCommandExecute);
        MoveUsersCommand = new RelayCommand(OnMoveUsersCommandExecute, CanMoveUsersCommandExecuted);
    }
}