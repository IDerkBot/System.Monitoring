using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SystemMonitoringNetCore.Models;

namespace SystemMonitoringNetCore.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для Cultures.xaml
    /// </summary>
    public partial class CulturesPage
    {
        public CulturesPage()
        {
            InitializeComponent();
            foreach (var item in Db.DbContext.Cultures.ToList()) DgCultures.Items.Add(item);
            foreach (var item in Db.DbContext.Cultures.Select(x => x.Name).Distinct().ToList()) { SelectCultureSeed.Items.Add(item); }
            foreach (var item in Db.DbContext.Cultures.Select(x => x.Status).Distinct().ToList()) { SelectCultureStatus.Items.Add(item); }
        }
        void SelectCultureSeed_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectCultureStatus.IsEnabled = true;
            var list = Db.DbContext.Cultures.Where(x => x.Name == SelectCultureSeed.SelectedItem.ToString()).Select(x => x.Status).Distinct().ToList();
            MessageBox.Show($"{string.Join(", ", list)} - {list.Count} ${list[0]}$ - {string.IsNullOrEmpty(list[0])}");
            if (!string.IsNullOrEmpty(list[0])) { SelectCultureStatus.Items.Clear(); foreach (var item in list) { SelectCultureStatus.Items.Add(item); } }
            else SelectCultureStatus.IsEnabled = false;
        }
        void FilterCulture_Click(object sender, RoutedEventArgs e)
        {
            var selectSeed = SelectCultureSeed.SelectedItem.ToString();
            if (selectSeed != "")
            {
                if (SelectCultureStatus.IsEnabled)
                {
                    var selectStatus = (SelectCultureStatus.SelectedItem != null) ? SelectCultureStatus.SelectedItem.ToString() : "";
                    if (selectStatus != "")
                    {
                        DgCultures.Items.Clear();
                        foreach (var item in Db.DbContext.Cultures.Where(x => x.Name == selectSeed && x.Status == selectStatus).ToList())
                        {
                            DgCultures.Items.Add(item);
                        }
                    }
                    else
                    {
                        DgCultures.Items.Clear();
                        foreach (var item in Db.DbContext.Cultures.Where(x => x.Name == selectSeed).ToList())
                        {
                            DgCultures.Items.Add(item);
                        }
                    }
                }
                else
                {
                    DgCultures.Items.Clear();
                    foreach (var item in Db.DbContext.Cultures.Where(x => x.Name == selectSeed).ToList())
                    {
                        DgCultures.Items.Add(item);
                    }
                }
            }
            else{
                MessageBox.Show($@"Вы не выбрали параметры фильтрации");
            }
        }
        void Edit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAdd_OnClick(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
