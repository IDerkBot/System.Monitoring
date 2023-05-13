using System.Linq;
using SystemMonitoringNetCore.Models;

namespace SystemMonitoringNetCore.Views.Pages;

public partial class UsersPage
{
    public UsersPage()
    {
        InitializeComponent();
        DgUser.ItemsSource = Db.DbContext.Users.ToList();
    }
}