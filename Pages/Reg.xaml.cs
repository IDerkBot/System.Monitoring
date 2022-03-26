using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SystemMonitoring.Classes;
using SystemMonitoring.Models.Entity;
using SystemMonitoring.Pages;

namespace SystemMonitoring.AuthReg
{
    public partial class Reg : Page
    {
        public Reg() { InitializeComponent(); }

        void RegIn_Click(object sender, RoutedEventArgs e)
        {
            string pass = PbPassword.Password;
            string pass_confrim = PbConfirm.Password;
            if(pass == pass_confrim)
            {
                int user_count = dbMonitoringEntities.gc().Users.Where(x => x.Login == TbLogin.Text).Count();
                if (user_count == 0)
                {
                    User user = new User() { Login = TbLogin.Text, Password = PbPassword.Password, Access = 1 };
                    dbMonitoringEntities.gc().Users.Add(user);
                    dbMonitoringEntities.gc().SaveChanges();
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
        void Back_Click(object sender, RoutedEventArgs e) { ManagerPage.Page.Navigate(new Auth()); }
    }
}