using System.Linq;
using System.Windows;
using System.Windows.Input;
using Monitoring.Models.Entity;
using SystemMonitoringNetCore.Models;

namespace SystemMonitoringNetCore.Views.Pages.EditPages
{
    public partial class AddField
    {
        private readonly District _selectedDistrict;
        public AddField(District district)
        {
            InitializeComponent();
            _selectedDistrict = district;
        }
        private void Cancel_Click(object sender, RoutedEventArgs e) => ManagerPage.Page.GoBack();
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if(!Db.DbContext.Fields.ToList().Any(x => x.District == _selectedDistrict && x.Number == TbFieldNumber.Text))
            {
                Db.DbContext.Fields.Add(new Field { District = _selectedDistrict, Number = TbFieldNumber.Text });
                Db.DbContext.SaveChanges();
                ManagerPage.Page.GoBack();
            } else MessageBox.Show(@"К данному району уже присоединено поле с таким номером");
        }

        private void TbFieldNumber_OnKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Back) return;
            if (TbFieldNumber.Text.Length is 2 or 5 or 13) TbFieldNumber.Text += ':';
            TbFieldNumber.Focus();
            TbFieldNumber.CaretIndex = TbFieldNumber.Text.Length;
        }
    }
}