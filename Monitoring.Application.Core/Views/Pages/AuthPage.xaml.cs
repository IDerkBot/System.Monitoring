using System.Linq;
using System.Windows;
using System.Windows.Input;
using SystemMonitoringNetCore.Models;

namespace SystemMonitoringNetCore.Views.Pages
{
	/// <summary>
	/// 
	/// </summary>
	public partial class AuthPage
	{
		/// <summary>
		/// 
		/// </summary>
		public AuthPage() { InitializeComponent(); }

		private bool CheckAuthData()
		{
			if (Db.DbContext.Users.ToList().Any(x => x.Login == TbLogin.Text && x.Password == PbPassword.Password))
				return false;
			MessageBox.Show("Логин или пароль не верны");
			return true;
		}
		private void RememberData()
		{
			var settings = new Settings
			{
				Remember = true,
				Login = TbLogin.Text,
				Password = PbPassword.Password
			};
			//settings.Save(FileManager.GetPathConfig());
		}

		// Событие при нажатии на кнопку войти
		private void LogInBtn_Click(object sender, RoutedEventArgs? e)
		{
			// Проверка на пустоту
			// Поиск пользователя и правильность введенных данных
			if (CheckAuthData()) return;
			// Если установлен чек бокс - запомнить
			if (CbRemember.IsChecked == true) RememberData();
			// Переместить на страницу меню
			ManagerPage.Navigate(new MenuPage());
		}

		// Событие при нажатии на клавишу
		private void KeyPress(object sender, KeyEventArgs e) { if (e.Key == Key.Enter) LogInBtn_Click(LogInBtn, null); }

		// Загрузка данных при открытии приложения
		private void OnLoad(object sender, RoutedEventArgs e)
		{
			//var settings = FileManager.GetSettings();
			//if (!settings.Remember) return;
			//TbLogin.Text = settings.Login;
			//PbPassword.Password = settings.Password;
			//CbRemember.IsChecked = settings.Remember;
		}
		// Событие на нажатие кнопки регистрация
		private void RegInBtn_Click(object sender, RoutedEventArgs e) => ManagerPage.Page.Navigate(new RegPage());
	}
}