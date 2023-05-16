using System.Windows;
using Monitoring.Models.Entity;
using SystemMonitoringNetCore.Models;

namespace SystemMonitoringNetCore.Views.Pages.EditPages
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

            Db.DbContext.Districts.Add(new District { Name = DistrictName.Text, LocationX = 0, LocationY = 0 });
            ManagerPage.Page.GoBack();
        }
    }
}