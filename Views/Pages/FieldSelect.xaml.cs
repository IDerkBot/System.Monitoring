using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SystemMonitoring.Models;
using SystemMonitoring.Models.Entity;
using SystemMonitoring.Pages;

namespace SystemMonitoring.Views.Pages
{
    public partial class FieldSelect
	{
		#region OnLoad
		public FieldSelect() => InitializeComponent();

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			CbDistrict.ItemsSource = u519667_monitoringEntities.gc().Fields
				.GroupBy(x => x.District).Select(x => x.Key).ToList();

			// TODO WHat?
			if (string.IsNullOrWhiteSpace(Db.DistrictName)) return;
			var districts = u519667_monitoringEntities.gc().Fields
				.Select(x => x.District).ToList();
			districts.Add(Db.DistrictName);
			CbDistrict.ItemsSource = districts;
			CbDistrict.SelectedItem = Db.DistrictName;
			var fields = u519667_monitoringEntities.gc().Fields
				.Where(x => x.District == Db.DistrictName).ToList();
			if (fields.Any())
				CbField.ItemsSource = fields.Select(x => x.Number);
		}

		#endregion

		#region Select Change

		private void DistrictSelectChanged(object sender, SelectionChangedEventArgs e)
		{
			SpFieldNumber.IsEnabled = true;
			var selectDistrict = CbDistrict.SelectedValue.ToString();
			var fields = u519667_monitoringEntities.gc().Fields.ToList();
            CbField.ItemsSource = fields.Where(x => x.District == selectDistrict);
        }
		private void FieldDistrictChanged(object sender, SelectionChangedEventArgs e) { BtnNext.IsEnabled = true; }

		#endregion

		#region Buttons Event

		// Нажатие на кнопку вперед
		private void BtnNext_OnClick(object sender, RoutedEventArgs e)
		{
			// Заносим в переменную field выбранный элемент, который приводим к классу Field
			var field = CbField.SelectedItem as Field;

			// создаем переменную seed, которую в дальнейшем будем передавать
			var seed = new Seed { Field = field, IDField = field.ID };

			// проверяем, есть ли в таблице seed элементы, которые содержат выбранное поле
			if (u519667_monitoringEntities.gc().Seeds.Any(x => x.IDField == seed.IDField))
				// есть - заносим
				seed = u519667_monitoringEntities.gc().Seeds.Single(x => x.IDField == seed.IDField);
			else
			{
				// нет - добавляем новый в бд
				u519667_monitoringEntities.gc().Seeds.Add(seed);
				u519667_monitoringEntities.gc().SaveChanges();
				// И получаем его после занесения
				seed = u519667_monitoringEntities.gc().Seeds.Single(x => x.IDField == seed.IDField);
			}
			// Его мы сохраняем как глобальную переменную
			Db.SelectSeeding = seed;
			//ManagerPage.FieldMonitoringPage = new FieldMonitoring();
			// и переходим на страницу мониторинга
			ManagerPage.Navigate(new FieldMonitoring(seed));
		}
		private void AddDistrict_Click(object sender, RoutedEventArgs e) { ManagerPage.Navigate(new SystemMonitoring.Pages.PagesAdd.AddDistrict()); }
		private void AddField_Click(object sender, RoutedEventArgs e) { ManagerPage.Navigate(new AddField(CbDistrict.SelectedItem.ToString())); }

		#endregion
	}
}