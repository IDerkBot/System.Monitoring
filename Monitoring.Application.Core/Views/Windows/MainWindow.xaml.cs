using System;
using System.Windows;
using System.Windows.Input;
using Monitoring.DataAccessLayer;
using SystemMonitoringNetCore.Models;
using SystemMonitoringNetCore.Views.UserControls;

namespace SystemMonitoringNetCore.Views.Windows
{
    /// <summary> Interaction logic for MainWindow.xaml </summary>
    public partial class MainWindow
    {
        /// <summary>  </summary>
        public MainWindow()
        {
            InitializeComponent();
            Db.DbContext = new ApplicationDbContext();
            ManagerPage.Page = MainPage;
            ManagerPage.Navigate(new AuthControl());
        }

        private void MainPage_ContentRendered(object sender, EventArgs e)
        {
            if (ManagerPage.Page.Content.ToString()!.Contains("Auth") || ManagerPage.Page.Content.ToString()!.Contains("AdminMenu"))
                Menu.Visibility = Visibility.Hidden;
            else Menu.Visibility = Visibility.Visible;

            if (ManagerPage.Page.Content.ToString()!.Contains("Menu")) MenuBack.Visibility = Visibility.Collapsed;
            else MenuBack.Visibility = Visibility.Visible;
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

        private void MainWindow_OnMouseMove(object sender, MouseEventArgs e)
        {
        }
    }
}