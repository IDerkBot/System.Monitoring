using System;
using System.Windows;
using Monitoring.DataAccessLayer;
using SystemMonitoringNetCore.Models;
using Auth = SystemMonitoringNetCore.Views.Pages.Auth;

namespace SystemMonitoringNetCore.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Db.DbContext = new ApplicationDbContext();
            ManagerPage.Page = MainPage;
            ManagerPage.Navigate(new Auth());
        }

        private void MainPage_ContentRendered(object sender, EventArgs e)
        {
            if (ManagerPage.Page.Content.ToString()!.Contains("Auth") || ManagerPage.Page.Content.ToString()!.Contains("AdminMenu"))
                Back.Visibility = Visibility.Hidden;
            else Back.Visibility = Visibility.Visible;
            // TODO WHaT?
            // if (ManagerPage.Page.Content.ToString().Contains("FieldMonitoring"))
            //     ManagerPage.FieldMonitoringPage.NavigateLoad();
        }
        private void ChangedSizeWindow(object sender, SizeChangedEventArgs e)
        {
            Db.SizeWindow = WindowState == WindowState.Maximized ? SystemParameters.PrimaryScreenWidth : Width;
        }
        private void MainW_Closed(object sender, EventArgs e)
        {
            // if (ManagerPage.Page.Content.ToString().Contains("FieldMonitoring")) ManagerPage.FieldMonitoringPage.ClosePort();
            Close();
        }

        private void Back_OnClick(object sender, RoutedEventArgs e)
        {
            ManagerPage.Page.GoBack();
        }
    }
}