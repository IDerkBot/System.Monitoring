using System.Windows.Controls;

namespace SystemMonitoring.Controls
{
    public partial class SensorDetails : UserControl
    {
        public SensorDetails()
        {
            InitializeComponent();
            DataContext = this;
        }
        public string ID
        {
            get => SensorID.Text;
            set => SensorID.Text = value;
        }
        public string Humidity
        {
            get => SensorHum.Text;
            set => SensorHum.Text = value;
        }
        public string Temperature
        {
            get => SensorTemp.Text;
            set => SensorTemp.Text = value;
        }
        public string Acidity
        {
            get => SensorAcid.Text;
            set => SensorAcid.Text = value;
        }
        public string Phosphorus
        {
            get => SensorPhos.Text;
            set => SensorPhos.Text = value;
        }
        public string Calcium
        {
            get => SensorCalc.Text;
            set => SensorCalc.Text = value;
        }
        public string Magnesium
        {
            get => SensorMagn.Text;
            set => SensorMagn.Text = value;
        }
        public string Calium
        {
            get => SensorCalm.Text;
            set => SensorCalm.Text = value;
        }
        public string Asot
        {
            get => SensorAsot.Text;
            set => SensorAsot.Text = value;
        }
        public string Recomendation
        {
            get => recom;
            set { recom = value; }
        }
        string recom;
        public int PercentDeviation
        {
            get => percentDeviation;
            set { percentDeviation = value; }
        }
        int percentDeviation;
        public string Culture
        {
            get => culture;
            set => culture = value;
        }
        string culture;
        //void Edit_Click(object sender, RoutedEventArgs e)
        //{
        //    Sensor sensor = new Sensor { ID = ID, Acidity = Acidity, Asot = Asot, Calcium = Calcium, Calium = Calium, Humidity = Humidity, Magnesium = Magnesium, Phosphorus = Phosphorus, Temperature = Temperature };
        //    ManagerPage.Page.Navigate(new AdminEditPages.AddSensor(sensor));
        //}
        //void SensorRecom_Click(object sender, RoutedEventArgs e)
        //{ System.Windows.Forms.MessageBox.Show($@"{recom}"); }
    }
}