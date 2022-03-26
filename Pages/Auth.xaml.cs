using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SystemMonitoring.Classes;
using SystemMonitoring.Models.Entity;

namespace SystemMonitoring.Pages
{
	public partial class Auth : Page
	{
		public Auth() { InitializeComponent(); }
		private bool CheckFieldInEmpty()
		{
			if (LoginTB.Text.Length <= 4 || string.IsNullOrWhiteSpace(LoginTB.Text))
			{
				MessageBox.Show("В поле логин введено мало символов!\nЛогин должен состоять из 5 и больше символов.", "Error");
				LoginTB.Background = ColorFlat.Error;
				LoginTB.Foreground = ColorFlat.White;
				return true;
			}

			if (PasswordPB.Password.Length >= 8 && !string.IsNullOrWhiteSpace(PasswordPB.Password)) return false;
			MessageBox.Show("В поле пароль введено мало символов!\nПароль должен состоять из 8 и больше символов.", "Error");
			PasswordPB.Background = ColorFlat.Error;
			PasswordPB.Foreground = ColorFlat.White;
			return true;
		}
		private bool CheckAuthData()
		{
			if (dbMonitoringEntities.gc().Users.Any(x => x.Login == LoginTB.Text && x.Password == PasswordPB.Password))
				return false;
			MessageBox.Show("Логин или пароль не верны");
			return true;
		}
		private void RememberData()
		{
			var settings = FileManager.GetSettings();
			settings.Remember = true;
			settings.Login = LoginTB.Text;
			settings.Password = PasswordPB.Password;
			FileManager.SetSettings(settings);
		}
		private void LogInBtn_Click(object sender, RoutedEventArgs e)
		{
			if (CheckFieldInEmpty()) return;
			if (CheckAuthData()) return;
			if (RememberCB.IsChecked == true) RememberData();
			ManagerPage.Navigate(new AdminMenu());
		}
		private void KeyPress(object sender, KeyEventArgs e) { if (e.Key == Key.Enter) LogInBtn_Click(LogInBtn, null); }
		private void OnLoad(object sender, RoutedEventArgs e)
		{
			var settings = FileManager.GetSettings();
			if (!settings.Remember) return;
			LoginTB.Text = settings.Login;
			PasswordPB.Password = settings.Password;
			RememberCB.IsChecked = settings.Remember;
		}
		private void RegInBtn_Click(object sender, RoutedEventArgs e) => ManagerPage.Page.Navigate(new AuthReg.Reg());
	}
}