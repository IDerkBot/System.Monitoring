using System;
using System.Windows;
using SystemMonitoring.Classes;
using SystemMonitoring.Models;
using SystemMonitoring.Pages;

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
            if (ManagerPage.Page.Content.ToString().Contains("FieldMonitoring"))
                ManagerPage.FieldMonitoringPage.NavigateLoad();
        }
        private void ChangedSizeWindow(object sender, SizeChangedEventArgs e)
        {
            if(MainW.WindowState == WindowState.Maximized) 
                DB.SizeWindow = SystemParameters.PrimaryScreenWidth;
            else DB.SizeWindow = MainW.Width;
        }
        private void MainW_Closed(object sender, EventArgs e)
        {
            //if (ManagerPage.Page.Content.ToString().Contains("FieldMonitoring")) ManagerPage.FieldMonitoringPage.ClosePort();
            Close();
        }
    }
}