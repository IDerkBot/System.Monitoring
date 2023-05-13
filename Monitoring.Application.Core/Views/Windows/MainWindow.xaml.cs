using System;
using System.Windows;
using System.Windows.Input;
using Monitoring.DataAccessLayer;
using SystemMonitoringNetCore.Models;
using SystemMonitoringNetCore.Views.Pages;

namespace SystemMonitoringNetCore.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Db.DbContext = new ApplicationDbContext();
            ManagerPage.Page = MainPage;
            ManagerPage.Navigate(new AuthPage());
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

        private void MainWindow_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (ActualWidth < 1000)
            {
                MainImage.Visibility = Visibility.Collapsed;
                ColumnImage.Width = new GridLength(0);
            }
            else
            {
                MainImage.Visibility = Visibility.Visible;
                ColumnImage.Width = new GridLength(1, GridUnitType.Star);
            }
        }
    }
}