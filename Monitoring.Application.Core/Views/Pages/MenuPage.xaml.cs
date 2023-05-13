using System.Windows;
using SystemMonitoringNetCore.Models;

namespace SystemMonitoringNetCore.Views.Pages;

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
	private void BtnMoveReports_Click(object sender, RoutedEventArgs e) { ManagerPage.Navigate(new ReportsPage()); }
	private void BtnMoveCultures_Click(object sender, RoutedEventArgs e) { ManagerPage.Navigate(new CulturesPage()); }
	private void BtnMoveUsers_Click(object sender, RoutedEventArgs e) { ManagerPage.Navigate(new UsersPage()); }
}