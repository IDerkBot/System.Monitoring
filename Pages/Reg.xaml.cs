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
            string pass = Password.Password;
            string pass_confrim = Confrim_Password.Password;
            if(pass == pass_confrim)
            {
                int user_count = dbMonitoringEntities.gc().Users.Where(x => x.Login == Login.Text).Count();
                if (user_count == 0)
                {
                    User user = new User() { Login = Login.Text, Password = Password.Password, Access = 1 };
                    dbMonitoringEntities.gc().Users.Add(user);
                    dbMonitoringEntities.gc().SaveChanges();
                    System.Windows.Forms.MessageBox.Show(@"Вы успешно зарегистрировались");
                    ManagerPage.Page.Navigate(new Auth());
                }
                else
                {
                    Password.Clear();
                    Confrim_Password.Clear();
                    System.Windows.Forms.MessageBox.Show(@"Такой пользователь уже существует");
                }
            }
            else
            {
                Password.Clear();
                Confrim_Password.Clear();
                System.Windows.Forms.MessageBox.Show(@"Пароли не совпадают");
            }
        }
        void Back_Click(object sender, RoutedEventArgs e) { ManagerPage.Page.Navigate(new Auth()); }
    }
}