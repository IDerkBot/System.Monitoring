using System;
using System.Windows;
using SystemMonitoringNetCore.Models;

namespace SystemMonitoringNetCore.Views.Pages
{
    /// <summary>
    /// 
    /// </summary>
    public partial class AddDistrict
    {
        /// <summary>
        /// 
        /// </summary>
        public AddDistrict() { InitializeComponent(); }

        private void Cancel_Click(object sender, RoutedEventArgs e) { ManagerPage.Page.GoBack(); }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DistrictName.Text))
            {
                MessageBox.Show("Не верно введено название района");
                return;
            }
            Db.DistrictName = DistrictName.Text;
            ManagerPage.Page.GoBack();
        }
    }
}