using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SystemMonitoring.Classes;
using SystemMonitoring.Models;
using SystemMonitoring.Models.Entity;

namespace SystemMonitoring.Pages
{
	public partial class FieldSelect
	{
		public FieldSelect()
		{
			InitializeComponent();
			//CbDistrict.ItemsSource = ;
			CbDistrict.ItemsSource = dbMonitoringEntities.gc().Fields
				.GroupBy(x => x.District)
				.Select(x => x.Key).ToList();
		}
		private void DistrictSelectChanged(object sender, SelectionChangedEventArgs e)
		{
			SpFieldNumber.IsEnabled = true;
			var selectDistrict = (sender as ComboBox)?.SelectedItem.ToString();
			CbField.ItemsSource = dbMonitoringEntities.gc().Fields
				.Where(x => x.District.Contains(selectDistrict))
				.Select(x => x.Number).ToList();
		}
		private void FieldDistrictChanged(object sender, SelectionChangedEventArgs e) { BtnNext.IsEnabled = true; }

		// Нажатие на кнопку вперед
		private void Next_Click(object sender, RoutedEventArgs e)
		{
			var field = dbMonitoringEntities.gc().Fields
				.Single(x => x.District == CbDistrict.SelectedItem.ToString() && x.Number == CbField.SelectedItem.ToString());
			var seed = new Seed();
			if (dbMonitoringEntities.gc().Seeds.Where(x => x.IDField == field.ID).ToList().Any())
				seed = dbMonitoringEntities.gc().Seeds.Single(x => x.IDField == field.ID);
			else
			{
				seed.IDField = field.ID;
				dbMonitoringEntities.gc().Seeds.Add(seed);
				dbMonitoringEntities.gc().SaveChanges();
				seed = dbMonitoringEntities.gc().Seeds.Single(x => x.IDField == seed.IDField);
			}
			DB.SelectSeeding = seed;
			ManagerPage.FieldMonitoringPage = new FieldMonitoring();
			ManagerPage.Page.Navigate(ManagerPage.FieldMonitoringPage);
		}
		private void AddDistrict_Click(object sender, RoutedEventArgs e) { ManagerPage.Navigate(new PagesAdd.AddDistrict()); }
		private void AddField_Click(object sender, RoutedEventArgs e) { ManagerPage.Navigate(new AddField(CbDistrict.SelectedItem.ToString())); }
		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(DB.DistrictName)) return;
			var districts = dbMonitoringEntities.gc().Fields
				.Select(x => x.District).ToList();
			districts.Add(DB.DistrictName);
			CbDistrict.ItemsSource = districts;
			CbDistrict.SelectedItem = DB.DistrictName;
			var fields = dbMonitoringEntities.gc().Fields
				.Where(x => x.District == DB.DistrictName).ToList();
			if (fields.Any())
				CbField.ItemsSource = fields.Select(x => x.Number);
		}
	}
}