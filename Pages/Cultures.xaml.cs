using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SystemMonitoring.Models.Entity;

namespace SystemMonitoring.AdminPages
{
    /// <summary>
    /// Логика взаимодействия для Cultures.xaml
    /// </summary>
    public partial class Cultures : Page
    {
        public Cultures()
        {
            InitializeComponent();
            DGFertilizer.ItemsSource = dbMonitoringEntities.gc().Fertilizers.ToList();
            foreach (var item in dbMonitoringEntities.gc().Cultures.ToList()) DGCultures.Items.Add(item);
            foreach (var item in dbMonitoringEntities.gc().Cultures.Select(x => x.Name).Distinct().ToList()) { SelectCultureSeed.Items.Add(item); }
            foreach (var item in dbMonitoringEntities.gc().Cultures.Select(x => x.Status).Distinct().ToList()) { SelectCultureStatus.Items.Add(item); }
        }
        void SelectCultureSeed_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectCultureStatus.IsEnabled = true;
            var list = dbMonitoringEntities.gc().Cultures.Where(x => x.Name == SelectCultureSeed.SelectedItem.ToString()).Select(x => x.Status).Distinct().ToList();
            //System.Windows.Forms.MessageBox.Show($"{string.Join(", ", list)} - {list.Count()} ${list[0]}$ - {string.IsNullOrEmpty(list[0])}");
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
                        DGCultures.Items.Clear();
                        foreach (var item in dbMonitoringEntities.gc().Cultures.Where(x => x.Name == selectSeed && x.Status == selectStatus).ToList())
                        {
                            DGCultures.Items.Add(item);
                        }
                    }
                    else
                    {
                        DGCultures.Items.Clear();
                        foreach (var item in dbMonitoringEntities.gc().Cultures.Where(x => x.Name == selectSeed).ToList())
                        {
                            DGCultures.Items.Add(item);
                        }
                    }
                }
                else
                {
                    DGCultures.Items.Clear();
                    foreach (var item in dbMonitoringEntities.gc().Cultures.Where(x => x.Name == selectSeed).ToList())
                    {
                        DGCultures.Items.Add(item);
                    }
                }
            }
            else{
                System.Windows.Forms.MessageBox.Show($@"Вы не выбрали параметры фильтрации");
            }
        }
        void Edit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
