using System.Windows;
using SystemMonitoring.Classes;

namespace SystemMonitoring.Pages
{
	public partial class AdminMenu
	{
		public AdminMenu() { InitializeComponent(); }
		private void BtnMoveField_Click(object sender, RoutedEventArgs e) { ManagerPage.Navigate(new FieldSelect()); }
		private void BtnMoveReports_Click(object sender, RoutedEventArgs e) { ManagerPage.Navigate(new AdminPages.Reports()); }
		private void BtnMoveCultures_Click(object sender, RoutedEventArgs e) { ManagerPage.Navigate(new AdminPages.Cultures()); }
		private void BtnMoveUsers_Click(object sender, RoutedEventArgs e) { ManagerPage.Navigate(new AdminPages.Users()); }
	}
}