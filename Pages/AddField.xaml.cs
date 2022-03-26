using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SystemMonitoring.Classes;
using SystemMonitoring.Models.Entity;

namespace SystemMonitoring.Pages
{
    public partial class AddField : Page
    {
        readonly string _selectedDistrict;
        public AddField(string district)
        {
            InitializeComponent();
            _selectedDistrict = district;
        }
        void Cancel_Click(object sender, RoutedEventArgs e) { ManagerPage.Page.GoBack(); }
        void Add_Click(object sender, RoutedEventArgs e)
        {
            if(dbMonitoringEntities.gc().Fields.Where(x => x.District == _selectedDistrict && x.Number == FieldValue.Text).ToList().Count == 0)
            {
                dbMonitoringEntities.gc().Fields.Add(new Field { District = _selectedDistrict, Number = FieldValue.Text });
                dbMonitoringEntities.gc().SaveChanges();
                ManagerPage.Page.GoBack();
            } else System.Windows.Forms.MessageBox.Show(@"К данному району уже присоединено поле с таким номером");
        }

        private void FieldValue_OnTextChanged(object sender, TextChangedEventArgs e)
        {
	        
        }
    }
}