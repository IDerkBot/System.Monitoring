using System.Windows;
using SystemMonitoring.Classes;
using SystemMonitoring.Pages;

namespace SystemMonitoring
{
    public partial class App : Application
    {
        void BtnBackMove_Click(object sender, RoutedEventArgs e)
        {
            if (ManagerPage.Page.Content.ToString().Contains("FieldMonitoring"))
                ManagerPage.Navigate(new FieldSelect());
            if (ManagerPage.Page.Content.ToString().Contains("FieldSelect"))
                ManagerPage.Navigate(new AdminMenu());
            else if (ManagerPage.Page.CanGoBack)
                ManagerPage.Back();
        }
    }
}