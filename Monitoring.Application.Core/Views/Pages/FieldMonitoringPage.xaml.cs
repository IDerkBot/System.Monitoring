using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Monitoring.Models.Entity;
using Newtonsoft.Json;
using SystemMonitoring;
using SystemMonitoringNetCore.Controls;
using SystemMonitoringNetCore.Models;
using Sensor = SystemMonitoringNetCore.Models.Sensor;
using Excel = Microsoft.Office.Interop.Excel;

namespace SystemMonitoringNetCore.Views.Pages
{
    /// <summary>
    /// 
    /// </summary>
    public partial class FieldMonitoringPage
    {
        #region Variables

        public Sensor _newSensor = new(); // TODO WHaT?
        private readonly Seed _currentSeed;
        private int _count = 0;
        private SerialPort _currentPort;
        private readonly double _days;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public FieldMonitoringPage(Seed selectedSeed)
        {
            InitializeComponent();
            // Передаем выбранное поле
            _currentSeed = selectedSeed;
            // Его заносим в контекст
            DataContext = _currentSeed;
            CbCulture.ItemsSource = Db.DbContext.Cultures.GroupBy(x => x.Name).Select(x => x.Key).ToList();

            DgSensors.ItemsSource = GetTestSensors();
            // Если культура нул
            if (_currentSeed.Culture != null)
            {
                CbCulture.ItemsSource = Db.DbContext.Cultures.ToList().Select(x => x.Name).Distinct();
                CbCulture.SelectedItem = _currentSeed.Culture.Name;
                if (_currentSeed.Date == null)
                {
                    _currentSeed.Date = DateTime.Now;
                    Db.DbContext.Seeds.Add(_currentSeed);
                    Db.DbContext.SaveChanges();
                }

                _days = Math.Floor((DateTime.Now - _currentSeed.Date).TotalDays);
                List<Culture> cultures = Db.DbContext.Cultures.Where(x => x.Name == _currentSeed.Culture.Name).ToList();
                foreach (var cult in cultures)
                {
                    int perMin = int.Parse(cult.Period.Split('-')[0]);
                    int perMax = int.Parse(cult.Period.Split('-')[1]);
                    if (perMin <= _days && perMax >= _days)
                    {
                        Status.Content = cult.Status;
                        break;
                    }
                }
            }
            else
            {
                if (Db.DbContext.Cultures.Any())
                    CbCulture.ItemsSource = Db.DbContext.Cultures.Select(x => x.Name).Distinct();
            }

            Soil.ItemsSource = new List<string>
                { "Чернозем", "Тундровые", "Подзолистые", "Болотные", "Серые лесные", "Луговые" };
            Soil.SelectedItem = _currentSeed.Field.Position;
            // var js = JsonConvert.DeserializeObject<List<SensorDetails>>(
            //     File.ReadAllText($@"{FileManager.GetAppData()}\sensors.json"));
            Db.Child = new List<SensorDetails>();
            ArduinoPortOpen();
        }

        private List<Sensor> GetTestSensors()
        {
            var rand = new Random();
            return new List<Sensor>
            {
                // new Sensor { Id = 1, Acidity = rand.Next(0, 100), Asot = rand.Next(0, 100),  Calcium = rand.Next(0, 100), Calium = rand.Next(0, 100), Humidity = rand.Next(0, 100), Magnesium = rand.Next(0, 100), Phosphorus = rand.Next(0, 100), Temperature = rand.Next(0, 100), Recommendation = "Тестовый датчик 1" },
                // new Sensor { Id = 2, Acidity = rand.Next(0, 100), Asot = rand.Next(0, 100),  Calcium = rand.Next(0, 100), Calium = rand.Next(0, 100), Humidity = rand.Next(0, 100), Magnesium = rand.Next(0, 100), Phosphorus = rand.Next(0, 100), Temperature = rand.Next(0, 100), Recommendation = "Тестовый датчик 2" },
                // new Sensor { Id = 3, Acidity = rand.Next(0, 100), Asot = rand.Next(0, 100),  Calcium = rand.Next(0, 100), Calium = rand.Next(0, 100), Humidity = rand.Next(0, 100), Magnesium = rand.Next(0, 100), Phosphorus = rand.Next(0, 100), Temperature = rand.Next(0, 100), Recommendation = "Тестовый датчик 3" },
                // new Sensor { Id = 4, Acidity = rand.Next(0, 100), Asot = rand.Next(0, 100),  Calcium = rand.Next(0, 100), Calium = rand.Next(0, 100), Humidity = rand.Next(0, 100), Magnesium = rand.Next(0, 100), Phosphorus = rand.Next(0, 100), Temperature = rand.Next(0, 100), Recommendation = "Тестовый датчик 4" },
                // new Sensor { Id = 5, Acidity = rand.Next(0, 100), Asot = rand.Next(0, 100),  Calcium = rand.Next(0, 100), Calium = rand.Next(0, 100), Humidity = rand.Next(0, 100), Magnesium = rand.Next(0, 100), Phosphorus = rand.Next(0, 100), Temperature = rand.Next(0, 100), Recommendation = "Тестовый датчик 5" },
                // new Sensor { Id = 6, Acidity = rand.Next(0, 100), Asot = rand.Next(0, 100),  Calcium = rand.Next(0, 100), Calium = rand.Next(0, 100), Humidity = rand.Next(0, 100), Magnesium = rand.Next(0, 100), Phosphorus = rand.Next(0, 100), Temperature = rand.Next(0, 100), Recommendation = "Тестовый датчик 6" },
                // new Sensor { Id = 7, Acidity = rand.Next(0, 100), Asot = rand.Next(0, 100),  Calcium = rand.Next(0, 100), Calium = rand.Next(0, 100), Humidity = rand.Next(0, 100), Magnesium = rand.Next(0, 100), Phosphorus = rand.Next(0, 100), Temperature = rand.Next(0, 100), Recommendation = "Тестовый датчик 7" },
                // new Sensor { Id = 8, Acidity = rand.Next(0, 100), Asot = rand.Next(0, 100),  Calcium = rand.Next(0, 100), Calium = rand.Next(0, 100), Humidity = rand.Next(0, 100), Magnesium = rand.Next(0, 100), Phosphorus = rand.Next(0, 100), Temperature = rand.Next(0, 100), Recommendation = "Тестовый датчик 8" },
                // new Sensor { Id = 9, Acidity = rand.Next(0, 100), Asot = rand.Next(0, 100),  Calcium = rand.Next(0, 100), Calium = rand.Next(0, 100), Humidity = rand.Next(0, 100), Magnesium = rand.Next(0, 100), Phosphorus = rand.Next(0, 100), Temperature = rand.Next(0, 100), Recommendation = "Тестовый датчик 9" },
                // new Sensor { Id = 10, Acidity = rand.Next(0, 100), Asot = rand.Next(0, 100),  Calcium = rand.Next(0, 100), Calium = rand.Next(0, 100), Humidity = rand.Next(0, 100), Magnesium = rand.Next(0, 100), Phosphorus = rand.Next(0, 100), Temperature = rand.Next(0, 100), Recommendation = "Тестовый датчик 10" },
            };
        }

        #region ArduinoWorker

        private void ArduinoPortOpen()
        {
            var arduinoPortFound = false;
            try
            {
                var ports = SerialPort.GetPortNames();
                foreach (var port in ports)
                {
                    _currentPort = new SerialPort(port, 9600);
                    if (true)
                    {
                        arduinoPortFound = true;
                        break;
                    }
                }

                // if (!arduinoPortFound) { mb.Show(@"Подключенные датчики не обнаружены"); Refresh.IsEnabled = false; return; }
                _currentPort.BaudRate = 9600;
                _currentPort.DtrEnable = true;
                _currentPort.ReadTimeout = 2000;
                try
                {
                    _currentPort.Open();
                    _currentPort.DataReceived += CurrentPort_DataReceived;
                    //ReadDataArduino();
                }
                catch (Exception ex)
                {
                    /*mb.Show($@"Error 1:{ex.Message}");*/
                }
            }
            catch (Exception ex)
            {
                /*mb.Show($@"Error 2:{ex.Message}");*/
            }
        }

        void CurrentPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var str1 = _currentPort.ReadLine().Trim();
            var str2 = _currentPort.ReadLine().Trim();
            var str3 = _currentPort.ReadLine().Trim();
            var ID = "";
            var Humidity = "";
            var Temperature = "";
            if (str1.Contains("ID")) ID = str1.Split('=')[1];
            if (str2.Contains("ID")) ID = str2.Split('=')[1];
            if (str3.Contains("ID")) ID = str3.Split('=')[1];

            if (str1.Contains("Hum")) Humidity = str1.Split('=')[1];
            if (str2.Contains("Hum")) Humidity = str2.Split('=')[1];
            if (str3.Contains("Hum")) Humidity = str3.Split('=')[1];

            if (str1.Contains("Temp")) Temperature = str1.Split('=')[1];
            if (str2.Contains("Temp")) Temperature = str2.Split('=')[1];
            if (str3.Contains("Temp")) Temperature = str3.Split('=')[1];
            if (ID.Length < 2) ID = "0";
            if (Humidity.Length < 2) Humidity = "0";
            if (Temperature.Length < 2) Temperature = "0";
            if (Humidity.Contains('.')) Humidity = Humidity.Replace('.', ',');
            if (Temperature.Contains('.')) Temperature = Temperature.Replace('.', ',');
            var sensorDetails = new Sensor()
            {
                // Id = ID,
                // Humidity = Humidity,
                // Temperature = Temperature
            };
            if (Db.Child.Any(x => x.Id.Contains(ID))) AddSensor(sensorDetails);
            _currentPort.Close();
        }

        private void AddSensor(Sensor sensor)
        {
            Dispatcher.Invoke(() =>
            {
                var sd = new SensorDetails()
                {
                    // Id = sensor.Id,
                    // Temperature = sensor.Temperature,
                    // Humidity = sensor.Humidity,
                    // Calcium = sensor.Calcium,
                    // Calium = sensor.Calium,
                    // Asot = sensor.Asot,
                    // Acidity = sensor.Acidity,
                    // Phosphorus = sensor.Phosphorus,
                    // Culture = CbCulture.SelectedItem.ToString()
                };
                sd.Recomendation = GetRecommendation(sd);
                // AddSensor(sd);
            });
        }

        private void AddSensor( /*SensorDetails sensor*/)
        {
            _count++;
            Count.Text = _count.ToString();
            // Sensors.Children.Add(sensor);
        }

        private bool ArduinoDetected()
        {
            try
            {
                _currentPort.Open();
                var returnMessage = _currentPort.ReadLine();
                _currentPort.Close();
                return !string.IsNullOrEmpty(returnMessage);
            }
            catch (Exception ex)
            {
                /*mb.Show($@"Error 3:{ex.Message}"); return false;*/
            }

            return false;
        }

        public void ClosePort()
        {
            if (_currentPort.IsOpen) _currentPort.Close();
        }

        #endregion

        #region Buttons Event

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            UploadSensor();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            //currentPort.Close();
            // var list = new List<SensorDetails>();
            // for (var i = 0; i < Sensors.Children.Count; i++)
            // { list.Add(Sensors.Children[i] as SensorDetails); }
            // Db.Child = list;
            ManagerPage.Page.Navigate(new AddSensor());
        }

        private void Map_Click(object sender, RoutedEventArgs e)
        {
            ManagerPage.Page.Navigate(new Map());
        }

        private void SaveToExcel_Click(object sender, RoutedEventArgs e)
        {
            // var excelApp = new Excel.Application();
            // Excel._Workbook excelWorkBook = excelApp.Workbooks.Open($@"{FileManager.GetAppData()}Отчет-Шаблон.xlsx");
            // Excel.Worksheet excelWorkSheet = (Excel.Worksheet)excelWorkBook.Worksheets.Item[1];

            // Заполнение ячеек
            // Установка даты в отчет
            // excelWorkSheet.Cells[4, 2] = DateTime.Now.ToShortDateString() + " года";
            //
            // var field = Db.DbContext.Fields.Single(x => x.ID == Db.SelectSeeding.IDField);
            //
            // excelWorkSheet.Cells[8, 6] = field.District;
            // excelWorkSheet.Cells[9, 7] = field.Number;
            //
            // // Запись информации с датчиков
            // for (var i = 0; i < Sensors.Children.Count; i++)
            // {
            // 	if (!(Sensors.Children[i] is SensorDetails sensor)) continue;
            // 	excelWorkSheet.Cells[17 + i, 2] = sensor.ID;
            // 	excelWorkSheet.Cells[17 + i, 3] = sensor.Temperature;
            // 	excelWorkSheet.Cells[17 + i, 4] = sensor.Acidity;
            // 	excelWorkSheet.Cells[17 + i, 5] = sensor.Humidity;
            // 	excelWorkSheet.Cells[17 + i, 6] = sensor.Phosphorus;
            // 	excelWorkSheet.Cells[17 + i, 7] = sensor.Calium;
            // 	excelWorkSheet.Cells[17 + i, 8] = sensor.Magnesium;
            // 	excelWorkSheet.Cells[17 + i, 9] = sensor.Calcium;
            // 	excelWorkSheet.Cells[17 + i, 10] = sensor.Asot;
            // 	excelWorkSheet.Cells[17 + i, 11] = sensor.Recomendation;
            // }
            // // Сохранение файла
            // var settings = FileManager.GetSettings();
            // excelApp.ActiveWorkbook.SaveAs($@"{settings.ReportsPath}\Отчет-{DateTime.Now.ToShortDateString()}.xlsx");
            // // Закрытие процесса Excel
            // var etc = Process.GetProcesses();
            // foreach (var anti in etc)
            // 	if (anti.ProcessName.ToLower().Contains("excel")) anti.Kill();
            // mb.Show(@"Отчет успешно сохранен");
        }

        #endregion

        private void ChangeSize()
        {
            Sensors.Width = WindowWidth - 20;
            Sensors.Height = WindowHeight - 20 - 150;
        }

        private void AddSensor(List<SensorDetails> sensors)
        {
            Dispatcher.Invoke(() =>
            {
                Sensors.Children.Clear();
                _count = 0;
                foreach (var sensor in sensors)
                {
                    _count++;
                    Count.Text = _count.ToString();
                    sensor.Recomendation = GetRecommendation(sensor);
                    sensor.PercentDeviation = GetPercent(sensor);
                    if (sensor.PercentDeviation <= 15)
                        sensor.Background = new SolidColorBrush(Color.FromRgb(46, 204, 113));
                    else if (sensor.PercentDeviation > 15 && sensor.PercentDeviation <= 30)
                        sensor.Background = new SolidColorBrush(Color.FromRgb(243, 156, 18));
                    else if (sensor.PercentDeviation > 30)
                        sensor.Background = new SolidColorBrush(Color.FromRgb(231, 76, 60));
                    Sensors.Children.Add(sensor);
                }
            });
        }

        private void UploadSensor()
        {
            // TODO Вылетает!!!
            //if (DB.Child.Count > 0) await Task.Run(() => AddSensor(DB.Child));
        }

        public async Task NavigateLoad()
        {
            if (_newSensor.Humidity != null) MessageBox.Show($"{_newSensor}"); //AddSensor(sensor);
            ManagerPage.FieldMonitoringPage = this;
            ChangeSize();
            UploadSensor();
            ArduinoPortOpen();
            await Task.Run(() => UploadSensor());
            DataContext = _currentSeed;
            foreach (SensorDetails child in Db.Child)
            {
                // AddSensor(child);
            }
        }

        private string GetRecommendation(SensorDetails sensor)
        {
            var culture = Db.DbContext.Cultures.Single(x =>
                x.Name == CbCulture.SelectedItem.ToString() && x.Status == Status.Content.ToString());
            var recommendation = "";
            double _acidityMin = (culture.Ph.Contains("-"))
                ? Convert.ToDouble(culture.Ph.Split('-')[0])
                : Convert.ToDouble(culture.Ph);
            double _acidityMax = (culture.Ph.Contains("-"))
                ? Convert.ToDouble(culture.Ph.Split('-')[1])
                : Convert.ToDouble(culture.Ph);
            if (Convert.ToDouble(sensor.Acidity) == 0) recommendation = "";
            else if (Convert.ToDouble(sensor.Acidity) > _acidityMax) recommendation += "Необходимо добавить гипс.\n";
            else if (Convert.ToDouble(sensor.Acidity) < _acidityMin) recommendation += "Необходимо добавить известь.\n";

            double _temperatureMin = (culture.Temperature.Contains("-"))
                ? Convert.ToDouble(culture.Temperature.Split('-')[0])
                : Convert.ToDouble(culture.Temperature);
            double _temperatureMax = (culture.Temperature.Contains("-"))
                ? Convert.ToDouble(culture.Temperature.Split('-')[1])
                : Convert.ToDouble(culture.Temperature);
            double _tempMid = (_temperatureMax + _temperatureMin) / 2 - Convert.ToDouble(sensor.Temperature);
            if (Convert.ToDouble(sensor.Temperature) == 0) recommendation = "";
            else if (Convert.ToDouble(sensor.Temperature) > _temperatureMax)
                recommendation += $"Температура выше оптимального значения на {_tempMid}°C\n";
            else if (Convert.ToDouble(sensor.Temperature) < _temperatureMin)
                recommendation += $"Температура ниже оптимального значения на {_tempMid}°C\n";

            double _asotMin = (culture.Nitrogen.Contains("-"))
                ? Convert.ToDouble(culture.Nitrogen.Split('-')[0])
                : Convert.ToDouble(culture.Nitrogen);
            double _asotMax = (culture.Nitrogen.Contains("-"))
                ? Convert.ToDouble(culture.Nitrogen.Split('-')[1])
                : Convert.ToDouble(culture.Nitrogen);
            double _asotMid = (_asotMax + _asotMin) / 2 - Convert.ToDouble(sensor.Humidity);
            if (Convert.ToDouble(sensor.Asot) == 0) recommendation = "";
            else if (Convert.ToDouble(sensor.Asot) > _asotMax) recommendation += $"Азот в избытке на {_asotMid}мг/кг\n";
            else if (Convert.ToDouble(sensor.Asot) < _asotMin)
                recommendation += $"Рекомендуется внести азот на {_asotMid}мг/кг\n";

            double _phosphorMin = (culture.Phosphor.Contains("-"))
                ? Convert.ToDouble(culture.Phosphor.Split('-')[0])
                : Convert.ToDouble(culture.Phosphor);
            double _phosphorMax = (culture.Phosphor.Contains("-"))
                ? Convert.ToDouble(culture.Phosphor.Split('-')[1])
                : Convert.ToDouble(culture.Phosphor);
            double _phosMid = (_phosphorMax + _phosphorMin) / 2 - Convert.ToDouble(sensor.Humidity);
            if (Convert.ToDouble(sensor.Phosphorus) == 0) recommendation = "";
            else if (Convert.ToDouble(sensor.Phosphorus) > _phosphorMax)
                recommendation += $"Фосфор в избытке на {_phosMid}мг/кг\n";
            else if (Convert.ToDouble(sensor.Phosphorus) < _phosphorMin)
                recommendation += $"Рекомендуется внести фосфор на {_phosMid}мг/кг\n";

            double _humidMin = (culture.Humidity.Contains("-"))
                ? Convert.ToDouble(culture.Humidity.Split('-')[0])
                : Convert.ToDouble(culture.Humidity);
            double _humidMax = (culture.Humidity.Contains("-"))
                ? Convert.ToDouble(culture.Humidity.Split('-')[1])
                : Convert.ToDouble(culture.Humidity);
            double _humMid = (_humidMax + _humidMin) / 2 - Convert.ToDouble(sensor.Humidity);
            if (Convert.ToDouble(sensor.Humidity) == 0) recommendation = "";
            else if (Convert.ToDouble(sensor.Humidity) > _humidMax)
                recommendation += $"Влажность ниже оптимального на {_humMid}%\n";
            else if (Convert.ToDouble(sensor.Humidity) < _humidMin)
                recommendation += $"Влажность выше оптимального на {_humMid}%\n";

            double _magnMin = (culture.Magnesium.Contains("-"))
                ? Convert.ToDouble(culture.Magnesium.Split('-')[0])
                : Convert.ToDouble(culture.Magnesium);
            double _magnMax = (culture.Magnesium.Contains("-"))
                ? Convert.ToDouble(culture.Magnesium.Split('-')[1])
                : Convert.ToDouble(culture.Magnesium);
            double _magnMid = (_magnMax + _magnMin) / 2 - Convert.ToDouble(sensor.Humidity);
            if (Convert.ToDouble(sensor.Magnesium) == 0) recommendation = "";
            else if (Convert.ToDouble(sensor.Magnesium) > _magnMax)
                recommendation += $"Магний в избыткена {_magnMid}мг/кг\n";
            else if (Convert.ToDouble(sensor.Magnesium) < _magnMin)
                recommendation += $"Рекомендуется внести магний на {_magnMid}мг/кг\n";

            double _calcMin = (culture.Calcium.Contains("-"))
                ? Convert.ToDouble(culture.Calcium.Split('-')[0])
                : Convert.ToDouble(culture.Calcium);
            double _calcMax = (culture.Calcium.Contains("-"))
                ? Convert.ToDouble(culture.Calcium.Split('-')[1])
                : Convert.ToDouble(culture.Calcium);
            double _calcMid = (_calcMax + _calcMin) / 2 - Convert.ToDouble(sensor.Humidity);
            if (Convert.ToDouble(sensor.Calcium) == 0) recommendation = "";
            else if (Convert.ToDouble(sensor.Calcium) > _calcMax)
                recommendation += $"Кальций в избытке на {_calcMid}мг/кг\n";
            else if (Convert.ToDouble(sensor.Calcium) < _calcMin)
                recommendation += $"Рекомендуется внести кальций на {_calcMid}мг/кг\n";

            double _calMin = (culture.Potassium.Contains("-"))
                ? Convert.ToDouble(culture.Potassium.Split('-')[0])
                : Convert.ToDouble(culture.Potassium);
            double _calMax = (culture.Potassium.Contains("-"))
                ? Convert.ToDouble(culture.Potassium.Split('-')[1])
                : Convert.ToDouble(culture.Potassium);
            double _calMid = (_calMax + _calMin) / 2 - Convert.ToDouble(sensor.Humidity);
            if (Convert.ToDouble(sensor.Calium) == 0) recommendation = "";
            else if (Convert.ToDouble(sensor.Calium) > _calMax)
                recommendation += $"Калий в избытке на {_calMid}мг/кг\n";
            else if (Convert.ToDouble(sensor.Calium) < _calMin)
                recommendation += $"Рекомендуется внести калий на {_calMid}мг/кг\n";
            return recommendation;
            return "";
        }

        private void Write_Click(object sender, RoutedEventArgs e)
        {
            // List<SensorDetails> ls = new List<SensorDetails>();
            // ls.AddRange(Db.Child);
            // File.WriteAllText($@"{FileManager.GetAppData()}\sensors.json", JsonConvert.SerializeObject(ls));
        }

        private bool start = true;

        private void Culture_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!start)
            {
                _currentSeed.Culture = Db.DbContext.Cultures.Single(x =>
                    x.Name == CbCulture.SelectedItem.ToString() && x.Period.Contains("00"));
                CbCulture.IsEnabled = false;
                _currentSeed.Field = Db.DbContext.Fields.Single(x => x == _currentSeed.Field);
                _currentSeed.Id = Db.SelectSeeding.Id;
                var a1 = Db.DbContext.Fields.Single(x => x == Db.SelectSeeding.Field);
                var a2 = Db.DbContext.Cultures.Single(x =>
                    x.Name == CbCulture.SelectedItem.ToString() && x.Period.Contains("00"));
                var seeding = new Seed()
                {
                    Id = Db.SelectSeeding.Id,
                    Field = a1,
                    Culture = a2,
                    Date = DateTime.Now
                };
                Db.DbContext.Seeds.Add(seeding);
                Db.DbContext.SaveChanges();
            }
            else
            {
                start = !start;
            }
        }

        private void Date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentSeed.Date = (DateTime)(sender as DatePicker).SelectedDate;
            Db.DbContext.Seeds.Add(_currentSeed);
            Db.DbContext.SaveChanges();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(FileManager.GetSensorsJson(), JsonConvert.SerializeObject(Db.Child));
        }

        private void SaveToExcelSensors_Click(object sender, RoutedEventArgs e)
        {
            var list = JsonConvert.DeserializeObject<List<SensorDetails>>(
                File.ReadAllText(FileManager.GetSensorsJson()));

            Excel.Application ExcelApp = new Excel.Application();
            Excel._Workbook ExcelWorkBook;
            Excel.Worksheet ExcelWorkSheet;
            ExcelWorkBook = ExcelApp.Workbooks.Open($@"{FileManager.GetAppData()}Отчет-Сенсор-Шаблон.xlsx");
            ExcelWorkSheet = (Excel.Worksheet)ExcelWorkBook.Worksheets.Item[1];

            // Заполнение ячеек
            // Установка даты в отчет
            ExcelWorkSheet.Cells[4, 2] = DateTime.Now.ToShortDateString() + " года";

            var field = Db.DbContext.Fields.Single(x => x == Db.SelectSeeding.Field);

            ExcelWorkSheet.Cells[8, 6] = field.District;
            ExcelWorkSheet.Cells[9, 7] = field.Number;
            ExcelWorkSheet.Cells[10, 5] = field.Position;

            ExcelWorkSheet.Cells[12, 4] = field.Number;
            ExcelWorkSheet.Cells[13, 4] = field.Position;

            foreach (var sensor in list)
            {
                ExcelWorkSheet.Cells[14, 4] = sensor.Id;

                var sensors = list.Where(x => x.Id == sensor.Id).ToList();
                for (int i = 0; i < sensors.Count(); i++)
                {
                    SensorDetails _sensor = sensors[i];
                    ExcelWorkSheet.Cells[17 + i, 3] = _sensor.Temperature;
                    ExcelWorkSheet.Cells[17 + i, 4] = _sensor.Acidity;
                    ExcelWorkSheet.Cells[17 + i, 5] = _sensor.Humidity;
                    ExcelWorkSheet.Cells[17 + i, 6] = _sensor.Phosphorus;
                    ExcelWorkSheet.Cells[17 + i, 7] = _sensor.Calium;
                    ExcelWorkSheet.Cells[17 + i, 8] = _sensor.Magnesium;
                    ExcelWorkSheet.Cells[17 + i, 9] = _sensor.Calcium;
                    ExcelWorkSheet.Cells[17 + i, 10] = _sensor.Asot;
                    ExcelWorkSheet.Cells[17 + i, 11] = _sensor.Recomendation;
                }

                // Сохранение файла
                var settings = FileManager.GetSettings();
                ExcelApp.ActiveWorkbook.SaveAs(
                    $@"{settings.ReportsPath}\Отчет-Сенсор{sensor.Id}-{DateTime.Now.ToShortDateString()}.xlsx");
                // Закрытие процесса Excel
                var etc = Process.GetProcesses();
                foreach (var anti in etc)
                    if (anti.ProcessName.ToLower().Contains("excel"))
                        anti.Kill();
                MessageBox.Show($@"Отчет по датчику №{sensor.Id} успешно сохранен");
            }
        }

        private int GetPercent(SensorDetails sensor)
        {
            int max = 0;
            var culture = Db.DbContext.Cultures
                .Where(x => x.Name == CbCulture.SelectedItem.ToString() && x.Status == Status.Content.ToString())
                .ToList().Single();

            double acidityMiddle = (culture.Ph.Contains('-'))
                ? (double.Parse(culture.Ph.Split('-')[0]) + double.Parse(culture.Ph.Split('-')[1])) / 2
                : double.Parse(culture.Ph);
            double acidityPercent = Math.Floor(Convert.ToDouble(sensor.Acidity) / acidityMiddle * 100);

            double asotMiddle = (culture.Nitrogen.Contains('-'))
                ? (double.Parse(culture.Nitrogen.Split('-')[0]) + double.Parse(culture.Nitrogen.Split('-')[1])) / 2
                : double.Parse(culture.Nitrogen);
            var asotPercent = Math.Floor(double.Parse(sensor.Asot) / asotMiddle * 100);

            double calciumMiddle = (culture.Calcium.Contains('-'))
                ? (double.Parse(culture.Calcium.Split('-')[0]) + double.Parse(culture.Calcium.Split('-')[1])) / 2
                : double.Parse(culture.Calcium);
            var calciumPercent = Math.Floor(double.Parse(sensor.Calcium) / calciumMiddle * 100);

            double caliumMiddle = (culture.Potassium.Contains('-'))
                ? (double.Parse(culture.Potassium.Split('-')[0]) + double.Parse(culture.Potassium.Split('-')[1])) / 2
                : double.Parse(culture.Potassium);
            var caliumPercent = Math.Floor(double.Parse(sensor.Calium) / caliumMiddle * 100);

            double humidityMiddle = (culture.Humidity.Contains('-'))
                ? (double.Parse(culture.Humidity.Split('-')[0]) + double.Parse(culture.Humidity.Split('-')[1])) / 2
                : double.Parse(culture.Humidity);
            var humidityPercent = Math.Floor(double.Parse(sensor.Humidity) / humidityMiddle * 100);

            double magniyMiddle = (culture.Magnesium.Contains('-'))
                ? (double.Parse(culture.Magnesium.Split('-')[0]) + double.Parse(culture.Magnesium.Split('-')[1])) / 2
                : double.Parse(culture.Magnesium);
            var magniyPercent = Math.Floor(double.Parse(sensor.Magnesium) / magniyMiddle * 100);

            double phosphorMiddle = (culture.Phosphor.Contains('-'))
                ? (double.Parse(culture.Phosphor.Split('-')[0]) + double.Parse(culture.Phosphor.Split('-')[1])) / 2
                : double.Parse(culture.Phosphor);
            var phosphorPercent = Math.Floor(double.Parse(sensor.Phosphorus) / phosphorMiddle * 100);

            double temperatureMiddle = (culture.Temperature.Contains('-'))
                ? (double.Parse(culture.Temperature.Split('-')[0]) + double.Parse(culture.Temperature.Split('-')[1])) /
                  2
                : double.Parse(culture.Temperature);
            var temperaturePercent = Math.Floor(double.Parse(sensor.Temperature) / temperatureMiddle * 100);

            if (acidityPercent > max) max = int.Parse(acidityPercent.ToString());
            if (asotPercent > max) max = int.Parse(asotPercent.ToString());
            if (calciumPercent > max) max = int.Parse(calciumPercent.ToString());
            if (caliumPercent > max) max = int.Parse(caliumPercent.ToString());
            if (humidityPercent > max) max = int.Parse(humidityPercent.ToString());
            if (magniyPercent > max) max = int.Parse(magniyPercent.ToString());
            if (phosphorPercent > max) max = int.Parse(phosphorPercent.ToString());
            if (temperaturePercent > max) max = int.Parse(temperaturePercent.ToString());

            return max;
        }
    }
}