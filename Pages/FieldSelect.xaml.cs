using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SystemMonitoring.Classes;

namespace SystemMonitoring.Pages
{
	public partial class FieldSelect
	{
		public FieldSelect()
		{
			InitializeComponent();
			//CBDistrict.ItemsSource = ;
			CBDistrict.ItemsSource = dbMonitoringEntities.gc().Fields
				.GroupBy(x => x.District)
				.Select(x => x.Key).ToList();
		}
		private void DistrictSelectChanged(object sender, SelectionChangedEventArgs e)
		{
			GB.IsEnabled = true;
			var selectDistrict = (sender as ComboBox)?.SelectedItem.ToString();
			CBField.ItemsSource = dbMonitoringEntities.gc().Fields
				.Where(x => x.District.Contains(selectDistrict))
				.Select(x => x.Number).ToList();
		}
		private void FieldDistrictChanged(object sender, SelectionChangedEventArgs e) { BtnNext.IsEnabled = true; }

		// Нажатие на кнопку вперед
		private void Next_Click(object sender, RoutedEventArgs e)
		{
			var field = dbMonitoringEntities.gc().Fields
				.Single(x => x.District == CBDistrict.SelectedItem.ToString() && x.Number == CBField.SelectedItem.ToString());
			var seed = new Seeding();
			if (dbMonitoringEntities.gc().Seedings.Where(x => x.IdField == field.Id).ToList().Any())
				seed = dbMonitoringEntities.gc().Seedings.Single(x => x.IdField == field.Id);
			else
			{
				seed.IdField = field.Id;
				dbMonitoringEntities.gc().Seedings.Add(seed);
				dbMonitoringEntities.gc().SaveChanges();
				seed = dbMonitoringEntities.gc().Seedings.Single(x => x.IdField == seed.IdField);
			}
			DB.SelectSeeding = seed;
			ManagerPage.FieldMonitoringPage = new FieldMonitoring();
			ManagerPage.Page.Navigate(ManagerPage.FieldMonitoringPage);
		}
		private void AddDistrict_Click(object sender, RoutedEventArgs e) { ManagerPage.Navigate(new PagesAdd.AddDistrict()); }
		private void AddField_Click(object sender, RoutedEventArgs e) { ManagerPage.Navigate(new AddField(CBDistrict.SelectedItem.ToString())); }
		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(DB.DistrictName)) return;
			var districts = dbMonitoringEntities.gc().Fields
				.Select(x => x.District).ToList();
			districts.Add(DB.DistrictName);
			CBDistrict.ItemsSource = districts;
			CBDistrict.SelectedItem = DB.DistrictName;
			var fields = dbMonitoringEntities.gc().Fields
				.Where(x => x.District == DB.DistrictName).ToList();
			if (fields.Any())
				CBField.ItemsSource = fields.Select(x => x.Number);
		}
	}
}