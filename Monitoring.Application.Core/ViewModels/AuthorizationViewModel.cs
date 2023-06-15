using System.Linq;
using System.Windows;
using System.Windows.Input;
using Arion.Style.Controls;
using Arion.Style.Controls.Enums;
using Monitoring.Models;
using SystemMonitoringNetCore.Infrastructure.Command;
using SystemMonitoringNetCore.Models;
using SystemMonitoringNetCore.ViewModels.Base;
using SystemMonitoringNetCore.Views.Pages;
using SystemMonitoringNetCore.Views.UserControls;
using Db = SystemMonitoringNetCore.Models.Db;
using FileManager = SystemMonitoringNetCore.Models.FileManager;
using Settings = SystemMonitoringNetCore.Models.Settings;

namespace SystemMonitoringNetCore.ViewModels;

public class AuthorizationViewModel : BaseViewModel
{
    #region Login : string - Логин пользователя

    private string _login;

    /// <summary> Логин пользователя </summary>
    public string Login
    {
        get => _login;
        set => SetField(ref _login, value);
    }

    #endregion Login

    #region Password : string - Пароль пользователя

    private string _password;

    /// <summary> Пароль пользователя </summary>
    public string Password
    {
        get => _password;
        set => SetField(ref _password, value);
    }

    #endregion Password

    #region RememberMe : bool - Запоминать данные пользователя

    private bool _rememberMe;

    /// <summary> Запоминать данные пользователя </summary>
    public bool RememberMe
    {
        get => _rememberMe;
        set => SetField(ref _rememberMe, value);
    }

    #endregion RememberMe

    #region Enter - Вход

    /// <summary> Вход </summary>
    public ICommand EnterCommand { get; }

    private void OnEnterCommandExecute(object parameter)
    {
        // Поиск пользователя и правильность введенных данных
        if (CheckAuthData()) return;
        // Если установлен чек бокс - запомнить
        if (RememberMe) RememberData();
        // Переместить на страницу меню
        ManagerPage.Navigate(new MenuControl());
    }

    private bool CanEnterCommandExecuted(object parameter) =>
        !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password);

    #endregion Enter

    #region Registration - Регистрация

    /// <summary> Регистрация </summary>
    public ICommand RegistrationCommand { get; }

    private void OnRegistrationCommandExecute(object parameter) => ManagerPage.Page.Navigate(new RegPage());

    private bool CanRegistrationCommandExecuted(object parameter)
    {
        return true;
    }

    #endregion Registration

    public AuthorizationViewModel()
    {
        EnterCommand = new RelayCommand(OnEnterCommandExecute, CanEnterCommandExecuted);
        RegistrationCommand = new RelayCommand(OnRegistrationCommandExecute, CanRegistrationCommandExecuted);
        
        var settings = FileManager.GetSettings();
        if (!settings.Remember) return;
        Login = settings.Login;
        RememberMe = settings.Remember;
    }
    
    private bool CheckAuthData()
    {
        if (Db.DbContext.Users.ToList().Any(x => x.Login == Login && x.Password == Password))
            return false;
        ModalDialog.Show("Ошибка авторизации", "Логин или пароль не верны", ModalDialogButtons.Ok, ModalDialogType.Warning);
        return true;
    }
    private void RememberData()
    {
        var settings = new Settings
        {
            Remember = true,
            Login = Login,
            Password = Password
        };
        settings.Save(FileManager.GetPathConfig());
    }
}