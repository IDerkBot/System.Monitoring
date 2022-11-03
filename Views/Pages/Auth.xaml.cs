using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using SystemMonitoring.Models;
using SystemMonitoring.Models.Entity;
using Page = System.Windows.Controls.Page;

namespace SystemMonitoring.Views.Pages
{
	public partial class Auth : Page
	{
		public Auth() { InitializeComponent(); }
		private bool CheckFieldInEmpty()
		{
			if (TbLogin.Text.Length <= 4 || string.IsNullOrWhiteSpace(TbLogin.Text))
			{
				MessageBox.Show("В поле логин введено мало символов!\nЛогин должен состоять из 5 и больше символов.", "Error");
				TbLogin.ChangeColor(ColorFlat.Error, ColorFlat.White);
				return true;
			}

			if (PbPassword.Password.Length >= 8 && !string.IsNullOrWhiteSpace(PbPassword.Password)) return false;
			MessageBox.Show("В поле пароль введено мало символов!\nПароль должен состоять из 8 и больше символов.", "Error");
			PbPassword.ChangeColor(ColorFlat.Error, ColorFlat.White);
			return true;
		}
		private bool CheckAuthData()
		{
			if (u519667_monitoringEntities.gc().Users.Any(x => x.Login == TbLogin.Text && x.Password == PbPassword.Password))
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

		// Событие при нажатии на кнопку войти
		private void LogInBtn_Click(object sender, RoutedEventArgs e)
		{
			// Проверка на пустоту
			if (CheckFieldInEmpty()) return;
			// Поиск пользователя и правильность введенных данных
			if (CheckAuthData()) return;
			// Если установлен чекбокс - запомнить
			if (CbRemember.IsChecked == true) RememberData();
			// Переместить на страницу меню
			ManagerPage.Navigate(new AdminMenu());
		}

		// Событие при нажатии на клавишу
		private void KeyPress(object sender, KeyEventArgs e) { if (e.Key == Key.Enter) LogInBtn_Click(LogInBtn, null); }

		// Загрузка данных при открытии приложения
		private void OnLoad(object sender, RoutedEventArgs e)
		{
			var settings = FileManager.GetSettings();
			if (!settings.Remember) return;
			TbLogin.Text = settings.Login;
			PbPassword.Password = settings.Password;
			CbRemember.IsChecked = settings.Remember;
		}
		// Событие на нажатие кнопки регистрация
		private void RegInBtn_Click(object sender, RoutedEventArgs e) => ManagerPage.Page.Navigate(new RegPage());
	}
}