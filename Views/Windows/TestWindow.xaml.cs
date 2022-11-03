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
using Mapsui.UI.Wpf;
using Mapsui.Utilities;

namespace SystemMonitoring.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Page
    {
        public TestWindow()
        {
            InitializeComponent();
            MyMap.Map.Layers.Add(OpenStreetMap.CreateTileLayer());

            //var map = new MapControl();

            //map.Map?.Layers.Add(Mapsui.);

            //Content = map;
        }
    }
}
