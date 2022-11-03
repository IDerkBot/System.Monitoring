using System.Windows;
using SystemMonitoring.Pages;
using SystemMonitoringNetCore.Models;

namespace SystemMonitoringNetCore.Views.Pages
{
	/// <summary>
	/// 
	/// </summary>
	public partial class MenuPage
	{
		/// <summary>
		/// 
		/// </summary>
		public MenuPage() { InitializeComponent(); }
		private void BtnMoveField_Click(object sender, RoutedEventArgs e) { ManagerPage.Navigate(new FieldSelectPage()); }
		private void BtnMoveReports_Click(object sender, RoutedEventArgs e) { ManagerPage.Navigate(new Reports()); }
		private void BtnMoveCultures_Click(object sender, RoutedEventArgs e) { ManagerPage.Navigate(new Cultures()); }
		private void BtnMoveUsers_Click(object sender, RoutedEventArgs e) { ManagerPage.Navigate(new Users()); }
	}
}