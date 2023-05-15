using System.Linq;
using System.Windows;
using Monitoring.Models.Entity;
using SystemMonitoringNetCore.Models;

namespace SystemMonitoringNetCore.Views.Pages;

/// <summary>
/// 
/// </summary>
public partial class RegPage
{
    /// <summary>
    /// 
    /// </summary>
    public RegPage() { InitializeComponent(); }

    private void RegIn_Click(object sender, RoutedEventArgs e)
    {
        var pass = PbPassword.Password;
        var passConfirm = PbConfirm.Password;
        if(pass == passConfirm)
        {
            var userCount = Db.DbContext.Users.Count(x => x.Login == TbLogin.Text);
            if (userCount == 0)
            {
                var user = new User { Login = TbLogin.Text, Password = PbPassword.Password, Access = 1 };
                Db.DbContext.Users.Add(user);
                Db.DbContext.SaveChanges();
                MessageBox.Show(@"Вы успешно зарегистрировались");
                ManagerPage.Page.GoBack();
            }
            else
            {
                PbPassword.Clear();
                PbConfirm.Clear();
                MessageBox.Show(@"Такой пользователь уже существует");
            }
        }
        else
        {
            PbPassword.Clear();
            PbConfirm.Clear();
            MessageBox.Show(@"Пароли не совпадают");
        }
    }
    private void Back_Click(object sender, RoutedEventArgs e) => ManagerPage.Page.GoBack();
}