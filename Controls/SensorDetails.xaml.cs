using System.Windows;
using System.Windows.Controls;
using SystemMonitoring.Classes;

namespace SystemMonitoring
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
            get { return SensorID.Text; }
            set => SensorID.Text = value;
        }
        public string Humidity
        {
            get { return SensorHum.Text; }
            set => SensorHum.Text = value;
        }
        public string Temperature
        {
            get => SensorTemp.Text;
            set => SensorTemp.Text = value;
        }
        public string Acidity
        {
            get { return SensorAcid.Text; }
            set => SensorAcid.Text = value;
        }
        public string Phosphorus
        {
            get { return SensorPhos.Text; }
            set => SensorPhos.Text = value;
        }
        public string Calcium
        {
            get { return SensorCalc.Text; }
            set => SensorCalc.Text = value;
        }
        public string Magniy
        {
            get { return SensorMagn.Text; }
            set => SensorMagn.Text = value;
        }
        public string Calium
        {
            get { return SensorCalm.Text; }
            set => SensorCalm.Text = value;
        }
        public string Asot
        {
            get { return SensorAsot.Text; }
            set => SensorAsot.Text = value;
        }
        public string Recomendation
        {
            get { return recom; }
            set { recom = value; }
        }
        string recom;
        public int PercentDeviation
        {
            get { return percentDeviation; }
            set { percentDeviation = value; }
        }
        int percentDeviation;
        public string Culture
        {
            get { return culture; }
            set => culture = value;
        }
        string culture;
        void Edit_Click(object sender, RoutedEventArgs e)
        {
            Sensor sensor = new Sensor { ID = ID, Acidity = Acidity, Asot = Asot, Calcium = Calcium, Calium = Calium, Humidity = Humidity, Magniy = Magniy, Phosphorus = Phosphorus, Temperature = Temperature };
            ManagerPage.Page.Navigate(new AdminEditPages.AddSensor(sensor));
        }
        void SensorRecom_Click(object sender, RoutedEventArgs e)
        { System.Windows.Forms.MessageBox.Show($@"{recom}"); }
    }
}