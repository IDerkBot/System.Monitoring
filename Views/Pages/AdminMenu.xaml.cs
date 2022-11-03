using System.Windows;
using SystemMonitoring.Models;
using SystemMonitoring.Pages;

namespace SystemMonitoring.Views.Pages
{
	public partial class AdminMenu
	{
		public AdminMenu() { InitializeComponent(); }
		private void BtnMoveField_Click(object sender, RoutedEventArgs e) { ManagerPage.Navigate(new FieldSelect()); }
		private void BtnMoveReports_Click(object sender, RoutedEventArgs e) { ManagerPage.Navigate(new Reports()); }
		private void BtnMoveCultures_Click(object sender, RoutedEventArgs e) { ManagerPage.Navigate(new Cultures()); }
		private void BtnMoveUsers_Click(object sender, RoutedEventArgs e) { ManagerPage.Navigate(new Users()); }
	}
}