using System.Collections.ObjectModel;
using System.Linq;
using Monitoring.Models.Entity;
using MvvmDialogs;
using SystemMonitoringNetCore.Models;
using SystemMonitoringNetCore.ViewModels.Base;

namespace SystemMonitoringNetCore.ViewModels;

public class UsersViewModel : BaseViewModel
{
    #region Users : ObservableCollection<User> - Список пользователей

    private ObservableCollection<User> _users;

    /// <summary> Список пользователей </summary>
    public ObservableCollection<User> Users
    {
        get => _users;
        set => SetField(ref _users, value);
    }

    #endregion Users

    public UsersViewModel(IDialogService dialogService)
    {
        Users = new ObservableCollection<User>(Db.DbContext.Users.ToList());
    }
}