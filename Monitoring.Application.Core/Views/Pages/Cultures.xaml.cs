using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SystemMonitoringNetCore.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для Cultures.xaml
    /// </summary>
    public partial class Cultures : Page
    {
        public Cultures()
        {
            InitializeComponent();
            // DGFertilizer.ItemsSource = u519667_monitoringEntities.gc().Fertilizers.ToList();
            // foreach (var item in u519667_monitoringEntities.gc().Cultures.ToList()) DGCultures.Items.Add(item);
            // foreach (var item in u519667_monitoringEntities.gc().Cultures.Select(x => x.Name).Distinct().ToList()) { SelectCultureSeed.Items.Add(item); }
            // foreach (var item in u519667_monitoringEntities.gc().Cultures.Select(x => x.Status).Distinct().ToList()) { SelectCultureStatus.Items.Add(item); }
        }
        void SelectCultureSeed_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectCultureStatus.IsEnabled = true;
            // var list = u519667_monitoringEntities.gc().Cultures.Where(x => x.Name == SelectCultureSeed.SelectedItem.ToString()).Select(x => x.Status).Distinct().ToList();
            // MessageBox.Show($"{string.Join(", ", list)} - {list.Count()} ${list[0]}$ - {string.IsNullOrEmpty(list[0])}");
            // if (!string.IsNullOrEmpty(list[0])) { SelectCultureStatus.Items.Clear(); foreach (var item in list) { SelectCultureStatus.Items.Add(item); } }
            // else SelectCultureStatus.IsEnabled = false;
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
                        DGCultures.Items.Clear();
                        // foreach (var item in u519667_monitoringEntities.gc().Cultures.Where(x => x.Name == selectSeed && x.Status == selectStatus).ToList())
                        // {
                        //     DGCultures.Items.Add(item);
                        // }
                    }
                    else
                    {
                        DGCultures.Items.Clear();
                        // foreach (var item in u519667_monitoringEntities.gc().Cultures.Where(x => x.Name == selectSeed).ToList())
                        // {
                        //     DGCultures.Items.Add(item);
                        // }
                    }
                }
                else
                {
                    DGCultures.Items.Clear();
                    // foreach (var item in u519667_monitoringEntities.gc().Cultures.Where(x => x.Name == selectSeed).ToList())
                    // {
                    //     DGCultures.Items.Add(item);
                    // }
                }
            }
            else{
                MessageBox.Show($@"Вы не выбрали параметры фильтрации");
            }
        }
        void Edit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
