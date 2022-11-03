using System.Linq;
using System.Windows;
using SystemMonitoring.Models;
using SystemMonitoring.Models.Entity;

namespace SystemMonitoring.Views.Pages
{
    public partial class RegPage
    {
        public RegPage() { InitializeComponent(); }

        private void RegIn_Click(object sender, RoutedEventArgs e)
        {
            var pass = PbPassword.Password;
            string passConfirm = PbConfirm.Password;
            if(pass == passConfirm)
            {
                var userCount = u519667_monitoringEntities.gc().Users.Count(x => x.Login == TbLogin.Text);
                if (userCount == 0)
                {
                    var user = new User { Login = TbLogin.Text, Password = PbPassword.Password, Access = 1 };
                    u519667_monitoringEntities.gc().Users.Add(user);
                    u519667_monitoringEntities.gc().SaveChanges();
                    System.Windows.Forms.MessageBox.Show(@"Вы успешно зарегистрировались");
                    ManagerPage.Page.Navigate(new Auth());
                }
                else
                {
                    PbPassword.Clear();
                    PbConfirm.Clear();
                    System.Windows.Forms.MessageBox.Show(@"Такой пользователь уже существует");
                }
            }
            else
            {
                PbPassword.Clear();
                PbConfirm.Clear();
                System.Windows.Forms.MessageBox.Show(@"Пароли не совпадают");
            }
        }
        private void Back_Click(object sender, RoutedEventArgs e) { ManagerPage.Page.Navigate(new Auth()); }
    }
}