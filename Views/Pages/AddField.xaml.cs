using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SystemMonitoring.Models;
using SystemMonitoring.Models.Entity;

namespace SystemMonitoring.Views.Pages
{
    public partial class AddField : Page
    {
        private readonly string _selectedDistrict;
        public AddField(string district)
        {
            InitializeComponent();
            _selectedDistrict = district;
        }
        private void Cancel_Click(object sender, RoutedEventArgs e) => ManagerPage.Page.GoBack();
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if(u519667_monitoringEntities.gc().Fields.Where(x => x.District == _selectedDistrict && x.Number == FieldValue.Text).ToList().Count == 0)
            {
	            u519667_monitoringEntities.gc().Fields.Add(new Field { District = _selectedDistrict, Number = FieldValue.Text });
	            u519667_monitoringEntities.gc().SaveChanges();
                ManagerPage.Page.GoBack();
            } else System.Windows.Forms.MessageBox.Show(@"К данному району уже присоединено поле с таким номером");
        }

        private void FieldValue_OnTextChanged(object sender, TextChangedEventArgs e)
        {
	        
        }
    }
}