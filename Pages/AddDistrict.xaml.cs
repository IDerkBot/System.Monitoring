using System;
using System.Windows;
using System.Windows.Controls;
using SystemMonitoring.Classes;
using SystemMonitoring.Models;

namespace SystemMonitoring.Pages.PagesAdd
{
    public partial class AddDistrict : Page
    {
        public AddDistrict() { InitializeComponent(); }
        void Cancel_Click(object sender, RoutedEventArgs e) { ManagerPage.Page.GoBack(); }
        void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DistrictName.Text)) throw new ArgumentNullException(nameof(DistrictName.Text));
            DB.DistrictName = DistrictName.Text;
            ManagerPage.Page.GoBack();
        }
    }
}