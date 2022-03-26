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
			if (TbLogin.Text.Length <= 4 || string.IsNullOrWhiteSpace(TbLogin.Text))
			{
				MessageBox.Show("В поле логин введено мало символов!\nЛогин должен состоять из 5 и больше символов.", "Error");
				TbLogin.Background = ColorFlat.Error;
				TbLogin.Foreground = ColorFlat.White;
				return true;
			}

			if (PbPassword.Password.Length >= 8 && !string.IsNullOrWhiteSpace(PbPassword.Password)) return false;
			MessageBox.Show("В поле пароль введено мало символов!\nПароль должен состоять из 8 и больше символов.", "Error");
			PbPassword.Background = ColorFlat.Error;
			PbPassword.Foreground = ColorFlat.White;
			return true;
		}
		private bool CheckAuthData()
		{
			if (dbMonitoringEntities.gc().Users.Any(x => x.Login == TbLogin.Text && x.Password == PbPassword.Password))
				return false;
			MessageBox.Show("Логин или пароль не верны");
			return true;
		}
		private void RememberData()
		{
			var settings = FileManager.GetSettings();
			settings.Remember = true;
			settings.Login = TbLogin.Text;
			settings.Password = PbPassword.Password;
			FileManager.SetSettings(settings);
		}
		private void LogInBtn_Click(object sender, RoutedEventArgs e)
		{
			if (CheckFieldInEmpty()) return;
			if (CheckAuthData()) return;
			if (CbRemember.IsChecked == true) RememberData();
			ManagerPage.Navigate(new AdminMenu());
		}
		private void KeyPress(object sender, KeyEventArgs e) { if (e.Key == Key.Enter) LogInBtn_Click(LogInBtn, null); }
		private void OnLoad(object sender, RoutedEventArgs e)
		{
			var settings = FileManager.GetSettings();
			if (!settings.Remember) return;
			TbLogin.Text = settings.Login;
			PbPassword.Password = settings.Password;
			CbRemember.IsChecked = settings.Remember;
		}
		private void RegInBtn_Click(object sender, RoutedEventArgs e) => ManagerPage.Page.Navigate(new AuthReg.Reg());
	}
}