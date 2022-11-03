using System;
using System.Windows;
using SystemMonitoring.Models;
using SystemMonitoring.Pages;
using SystemMonitoring.Views.Pages;

namespace SystemMonitoring
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			ManagerPage.Page = MainPage;
			ManagerPage.Navigate(new Auth());
		}

		private void MainPage_ContentRendered(object sender, EventArgs e)
		{
			if (ManagerPage.Page.Content.ToString().Contains("Auth") || ManagerPage.Page.Content.ToString().Contains("AdminMenu"))
				Back.Visibility = Visibility.Hidden;
			else Back.Visibility = Visibility.Visible;
			// TODO WHaT?
			//if (ManagerPage.Page.Content.ToString().Contains("FieldMonitoring"))
			//    ManagerPage.FieldMonitoringPage.NavigateLoad();
		}
		private void ChangedSizeWindow(object sender, SizeChangedEventArgs e)
		{
			Db.SizeWindow = MainW.WindowState == WindowState.Maximized ? SystemParameters.PrimaryScreenWidth : MainW.Width;
		}
		private void MainW_Closed(object sender, EventArgs e)
		{
			//if (ManagerPage.Page.Content.ToString().Contains("FieldMonitoring")) ManagerPage.FieldMonitoringPage.ClosePort();
			Close();
		}

		private void Back_OnClick(object sender, RoutedEventArgs e)
		{
			ManagerPage.Page.GoBack();
		}
	}
}