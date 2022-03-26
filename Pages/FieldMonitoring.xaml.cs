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
using System.IO.Ports;
using System.Timers;
using System.Threading;
using mb = System.Windows.Forms.MessageBox;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.Globalization;
using Newtonsoft.Json;
using System.IO;
using SystemMonitoring.Classes;
using SystemMonitoring.Models;
using SystemMonitoring.Models.Entity;

namespace SystemMonitoring.Pages
{
	public partial class FieldMonitoring : Page
	{
		#region Variables
		public Sensor _newSensor = new Sensor(); // TODO WHaT?
		private readonly Seed _selectedSeeding;
		private int _count = 0;
		private SerialPort _currentPort;
		private readonly double _days;
		#endregion
		// Init FieldMonitoring
		public FieldMonitoring()
		{
			InitializeComponent();
			// Передаем выбранное поле
			_selectedSeeding = dbMonitoringEntities.gc().Seeds.Single(x => x.IDField == DB.SelectSeeding.IDField);
			// Его заносим в контекст
			DataContext = _selectedSeeding;
				// Если культура нул
			if (_selectedSeeding.Culture != null)
			{

				CBCulture.ItemsSource = dbMonitoringEntities.gc().Cultures.ToList().Select(x => x.Name).Distinct();
				CBCulture.SelectedItem = _selectedSeeding.Culture.Name;
				if (_selectedSeeding.Date == null)
				{
					_selectedSeeding.Date = DateTime.Now.ToString(CultureInfo.InvariantCulture).Split(' ')[0];
					dbMonitoringEntities.gc().Seeds.Add(_selectedSeeding);
					dbMonitoringEntities.gc().SaveChanges();
				}
				_days = Math.Floor((DateTime.Now - DateTime.Parse(_selectedSeeding.Date)).TotalDays);
				List<Culture> cultures = dbMonitoringEntities.gc().Cultures.Where(x => x.Name == _selectedSeeding.Culture.Name).ToList();
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
				CBCulture.ItemsSource = dbMonitoringEntities.gc().Cultures.ToList().Select(x => x.Name).Distinct();
			}
			Soil.ItemsSource = new List<string> { "Чернозем", "Тундровые", "Подзолистые", "Болотные", "Серые лесные", "Луговые" };
			Soil.SelectedItem = _selectedSeeding.Field.Position;
			var js = JsonConvert.DeserializeObject<List<SensorDetails>>(File.ReadAllText($@"{FileManager.GetAppData()}\sensors.json"));
			DB.Childs = new List<SensorDetails>();
			ArduinoPortOpen();
		}
		#region Arduino
		
		private void ArduinoPortOpen()
		{
			
			var arduinoPortFound = false;
			try
			{
				var ports = SerialPort.GetPortNames();
				foreach (var port in ports)
				{
					_currentPort = new SerialPort(port, 9600);
					if (true) { arduinoPortFound = true; break; }
				}
				if (!arduinoPortFound) { mb.Show(@"Подключенные датчики не обнаружены"); Refresh.IsEnabled = false; return; }
				_currentPort.BaudRate = 9600;
				_currentPort.DtrEnable = true;
				_currentPort.ReadTimeout = 2000;
				try
				{
					_currentPort.Open();
					_currentPort.DataReceived += CurrentPort_DataReceived;
					//ReadDataArduino();
				}
				catch (Exception ex) { mb.Show($@"Error 1:{ex.Message}"); }
			}
			catch (Exception ex) { mb.Show($@"Error 2:{ex.Message}"); }
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
				ID = ID,
				Humidity = Humidity,
				Temperature = Temperature
			};
			if (DB.Childs.Any(x => x.ID.Contains(ID))) AddSensor(sensorDetails);
			_currentPort.Close();

		}

		private void AddSensor(Sensor sensor)
		{
			Dispatcher.Invoke(() =>
			{
				var sd = new SensorDetails()
				{
					ID = sensor.ID,
					Temperature = sensor.Temperature,
					Humidity = sensor.Humidity,
					Calcium = sensor.Calcium,
					Calium = sensor.Calium,
					Asot = sensor.Asot,
					Acidity = sensor.Acidity,
					Phosphorus = sensor.Phosphorus,
					Culture = CBCulture.SelectedItem.ToString()
				};
				sd.Recomendation = GetRecommendation(sd);
				AddSensor(sd);
			});
		}

		private void AddSensor(SensorDetails sensor)
		{
			_count++;
			Count.Text = _count.ToString();
			Sensors.Children.Add(sensor);
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
			catch (Exception ex) { mb.Show($@"Error 3:{ex.Message}"); return false; }
		}
		public void ClosePort()
		{
			if (_currentPort.IsOpen) _currentPort.Close();
		}
		#endregion
		#region Buttons Event

		private void Refresh_Click(object sender, RoutedEventArgs e) { UploadSensor(); }

		private void Add_Click(object sender, RoutedEventArgs e)
		{
			//currentPort.Close();
			var list = new List<SensorDetails>();
			for (var i = 0; i < Sensors.Children.Count; i++)
			{ list.Add(Sensors.Children[i] as SensorDetails); }
			DB.Childs = list;
			ManagerPage.Page.Navigate(new AdminEditPages.AddSensor());
		}

		private void Map_Click(object sender, RoutedEventArgs e) { ManagerPage.Page.Navigate(new Map()); }
		void SaveToExcel_Click(object sender, RoutedEventArgs e)
		{
			var excelApp = new Excel.Application();
			Excel._Workbook excelWorkBook = excelApp.Workbooks.Open($@"{FileManager.GetAppData()}Отчет-Шаблон.xlsx");
			Excel.Worksheet excelWorkSheet = (Excel.Worksheet)excelWorkBook.Worksheets.Item[1];

			// Заполнение ячеек
			// Установка даты в отчет
			excelWorkSheet.Cells[4, 2] = DateTime.Now.ToShortDateString() + " года";

			var field = dbMonitoringEntities.gc().Fields.Single(x => x.ID == DB.SelectSeeding.IDField);

			excelWorkSheet.Cells[8, 6] = field.District;
			excelWorkSheet.Cells[9, 7] = field.Number;

			// Запись информации с датчиков
			for (var i = 0; i < Sensors.Children.Count; i++)
			{
				if (!(Sensors.Children[i] is SensorDetails sensor)) continue;
				excelWorkSheet.Cells[17 + i, 2] = sensor.ID;
				excelWorkSheet.Cells[17 + i, 3] = sensor.Temperature;
				excelWorkSheet.Cells[17 + i, 4] = sensor.Acidity;
				excelWorkSheet.Cells[17 + i, 5] = sensor.Humidity;
				excelWorkSheet.Cells[17 + i, 6] = sensor.Phosphorus;
				excelWorkSheet.Cells[17 + i, 7] = sensor.Calium;
				excelWorkSheet.Cells[17 + i, 8] = sensor.Magniy;
				excelWorkSheet.Cells[17 + i, 9] = sensor.Calcium;
				excelWorkSheet.Cells[17 + i, 10] = sensor.Asot;
				excelWorkSheet.Cells[17 + i, 11] = sensor.Recomendation;
			}
			// Сохранение файла
			var settings = FileManager.GetSettings();
			excelApp.ActiveWorkbook.SaveAs($@"{settings.ReportsPath}\Отчет-{DateTime.Now.ToShortDateString()}.xlsx");
			// Закрытие процесса Excel
			var etc = Process.GetProcesses();
			foreach (var anti in etc)
				if (anti.ProcessName.ToLower().Contains("excel")) anti.Kill();
			mb.Show(@"Отчет успешно сохранен");
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
					if (sensor.PercentDeviation <= 15) sensor.Background = new SolidColorBrush(Color.FromRgb(46, 204, 113));
					else if (sensor.PercentDeviation > 15 && sensor.PercentDeviation <= 30) sensor.Background = new SolidColorBrush(Color.FromRgb(243, 156, 18));
					else if (sensor.PercentDeviation > 30) sensor.Background = new SolidColorBrush(Color.FromRgb(231, 76, 60));
					Sensors.Children.Add(sensor);
				}
			});
		}

		private async void UploadSensor()
		{
			if (DB.Childs.Count > 0) await Task.Run(() => AddSensor(DB.Childs));
		}
		public void NavigateLoad()
		{
			//if (_newSensor.Humidity != null) System.Windows.Forms.MessageBox.Show($"{_newSensor}"); //AddSensor(sensor);
			//ManagerPage.FieldMonitoringPage = this;
			ChangeSize();
			UploadSensor();
			//ArduinoPortOpen();
			//await Task.Run(() => UploadSensor());
			//DataContext = _selectedSeeding;
			//foreach (SensorDetails child in DB.Child) { AddSensor(child); }
		}

		private string GetRecommendation(SensorDetails sensor)
		{
			var culture = dbMonitoringEntities.gc().Cultures.Single(x => x.Name == CBCulture.SelectedItem.ToString() && x.Status == Status.Content.ToString());
			var recommendation = "";
			double _acidityMin = (culture.Ph.Contains("-")) ? Convert.ToDouble(culture.Ph.Split('-')[0]) : Convert.ToDouble(culture.Ph);
			double _acidityMax = (culture.Ph.Contains("-")) ? Convert.ToDouble(culture.Ph.Split('-')[1]) : Convert.ToDouble(culture.Ph);
			if (Convert.ToDouble(sensor.Acidity) == 0) recommendation = "";
			else if (Convert.ToDouble(sensor.Acidity) > _acidityMax) recommendation += "Необходимо добавить гипс.\n";
			else if (Convert.ToDouble(sensor.Acidity) < _acidityMin) recommendation += "Необходимо добавить известь.\n";

			double _temperatureMin = (culture.Temperature.Contains("-")) ? Convert.ToDouble(culture.Temperature.Split('-')[0]) : Convert.ToDouble(culture.Temperature);
			double _temperatureMax = (culture.Temperature.Contains("-")) ? Convert.ToDouble(culture.Temperature.Split('-')[1]) : Convert.ToDouble(culture.Temperature);
			double _tempMid = (_temperatureMax + _temperatureMin) / 2 - Convert.ToDouble(sensor.Temperature);
			if (Convert.ToDouble(sensor.Temperature) == 0) recommendation = "";
			else if (Convert.ToDouble(sensor.Temperature) > _temperatureMax) recommendation += $"Температура выше оптимального значения на {_tempMid}°C\n";
			else if (Convert.ToDouble(sensor.Temperature) < _temperatureMin) recommendation += $"Температура ниже оптимального значения на {_tempMid}°C\n";

			double _asotMin = (culture.Nitrogen.Contains("-")) ? Convert.ToDouble(culture.Nitrogen.Split('-')[0]) : Convert.ToDouble(culture.Nitrogen);
			double _asotMax = (culture.Nitrogen.Contains("-")) ? Convert.ToDouble(culture.Nitrogen.Split('-')[1]) : Convert.ToDouble(culture.Nitrogen);
			double _asotMid = (_asotMax + _asotMin) / 2 - Convert.ToDouble(sensor.Humidity);
			if (Convert.ToDouble(sensor.Asot) == 0) recommendation = "";
			else if (Convert.ToDouble(sensor.Asot) > _asotMax) recommendation += $"Азот в избытке на {_asotMid}мг/кг\n";
			else if (Convert.ToDouble(sensor.Asot) < _asotMin) recommendation += $"Рекомендуется внести азот на {_asotMid}мг/кг\n";

			double _phosphorMin = (culture.Phosphor.Contains("-")) ? Convert.ToDouble(culture.Phosphor.Split('-')[0]) : Convert.ToDouble(culture.Phosphor);
			double _phosphorMax = (culture.Phosphor.Contains("-")) ? Convert.ToDouble(culture.Phosphor.Split('-')[1]) : Convert.ToDouble(culture.Phosphor);
			double _phosMid = (_phosphorMax + _phosphorMin) / 2 - Convert.ToDouble(sensor.Humidity);
			if (Convert.ToDouble(sensor.Phosphorus) == 0) recommendation = "";
			else if (Convert.ToDouble(sensor.Phosphorus) > _phosphorMax) recommendation += $"Фосфор в избытке на {_phosMid}мг/кг\n";
			else if (Convert.ToDouble(sensor.Phosphorus) < _phosphorMin) recommendation += $"Рекомендуется внести фосфор на {_phosMid}мг/кг\n";

			double _humidMin = (culture.Humidity.Contains("-")) ? Convert.ToDouble(culture.Humidity.Split('-')[0]) : Convert.ToDouble(culture.Humidity);
			double _humidMax = (culture.Humidity.Contains("-")) ? Convert.ToDouble(culture.Humidity.Split('-')[1]) : Convert.ToDouble(culture.Humidity);
			double _humMid = (_humidMax + _humidMin) / 2 - Convert.ToDouble(sensor.Humidity);
			if (Convert.ToDouble(sensor.Humidity) == 0) recommendation = "";
			else if (Convert.ToDouble(sensor.Humidity) > _humidMax) recommendation += $"Влажность ниже оптимального на {_humMid}%\n";
			else if (Convert.ToDouble(sensor.Humidity) < _humidMin) recommendation += $"Влажность выше оптимального на {_humMid}%\n";

			double _magnMin = (culture.Magnesium.Contains("-")) ? Convert.ToDouble(culture.Magnesium.Split('-')[0]) : Convert.ToDouble(culture.Magnesium);
			double _magnMax = (culture.Magnesium.Contains("-")) ? Convert.ToDouble(culture.Magnesium.Split('-')[1]) : Convert.ToDouble(culture.Magnesium);
			double _magnMid = (_magnMax + _magnMin) / 2 - Convert.ToDouble(sensor.Humidity);
			if (Convert.ToDouble(sensor.Magniy) == 0) recommendation = "";
			else if (Convert.ToDouble(sensor.Magniy) > _magnMax) recommendation += $"Магний в избыткена {_magnMid}мг/кг\n";
			else if (Convert.ToDouble(sensor.Magniy) < _magnMin) recommendation += $"Рекомендуется внести магний на {_magnMid}мг/кг\n";

			double _calcMin = (culture.Calcium.Contains("-")) ? Convert.ToDouble(culture.Calcium.Split('-')[0]) : Convert.ToDouble(culture.Calcium);
			double _calcMax = (culture.Calcium.Contains("-")) ? Convert.ToDouble(culture.Calcium.Split('-')[1]) : Convert.ToDouble(culture.Calcium);
			double _calcMid = (_calcMax + _calcMin) / 2 - Convert.ToDouble(sensor.Humidity);
			if (Convert.ToDouble(sensor.Calcium) == 0) recommendation = "";
			else if (Convert.ToDouble(sensor.Calcium) > _calcMax) recommendation += $"Кальций в избытке на {_calcMid}мг/кг\n";
			else if (Convert.ToDouble(sensor.Calcium) < _calcMin) recommendation += $"Рекомендуется внести кальций на {_calcMid}мг/кг\n";

			double _calMin = (culture.Potassium.Contains("-")) ? Convert.ToDouble(culture.Potassium.Split('-')[0]) : Convert.ToDouble(culture.Potassium);
			double _calMax = (culture.Potassium.Contains("-")) ? Convert.ToDouble(culture.Potassium.Split('-')[1]) : Convert.ToDouble(culture.Potassium);
			double _calMid = (_calMax + _calMin) / 2 - Convert.ToDouble(sensor.Humidity);
			if (Convert.ToDouble(sensor.Calium) == 0) recommendation = "";
			else if (Convert.ToDouble(sensor.Calium) > _calMax) recommendation += $"Калий в избытке на {_calMid}мг/кг\n";
			else if (Convert.ToDouble(sensor.Calium) < _calMin) recommendation += $"Рекомендуется внести калий на {_calMid}мг/кг\n";
			return recommendation;
		}
		private void Write_Click(object sender, RoutedEventArgs e)
		{
			List<SensorDetails> ls = new List<SensorDetails>();
			ls.AddRange(DB.Childs);
			File.WriteAllText($@"{FileManager.GetAppData()}\sensors.json", JsonConvert.SerializeObject(ls));
		}
		private bool start = true;
		private void Culture_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!start)
			{
				_selectedSeeding.IDCulture = dbMonitoringEntities.gc().Cultures.Single(x => x.Name == CBCulture.SelectedItem.ToString() && x.Period.Contains("00")).ID;
				_selectedSeeding.Culture = dbMonitoringEntities.gc().Cultures.Single(x => x.Name == CBCulture.SelectedItem.ToString() && x.Period.Contains("00"));
				CBCulture.IsEnabled = false;
				_selectedSeeding.IDField = dbMonitoringEntities.gc().Fields.Single(x => x.ID == _selectedSeeding.IDField).ID;
				_selectedSeeding.Field = dbMonitoringEntities.gc().Fields.Single(x => x.ID == _selectedSeeding.IDField);
				_selectedSeeding.ID = DB.SelectSeeding.ID;
				var a1 = dbMonitoringEntities.gc().Fields.Single(x => x.ID == DB.SelectSeeding.IDField);
				var a2 = dbMonitoringEntities.gc().Cultures.Single(x => x.Name == CBCulture.SelectedItem.ToString() && x.Period.Contains("00"));
				var seeding = new Seed()
				{
					ID = DB.SelectSeeding.ID,
					Field = a1,
					Culture = a2,
					Date = DateTime.Now.ToString(CultureInfo.InvariantCulture).Split(' ')[0]
				};
				dbMonitoringEntities.gc().Seeds.Add(seeding);
				dbMonitoringEntities.gc().SaveChanges();
			}
			else
			{
				start = !start;
			}
		}
		private void Date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			_selectedSeeding.Date = (sender as DatePicker)?.SelectedDate.ToString();
			dbMonitoringEntities.gc().Seeds.Add(_selectedSeeding);
			dbMonitoringEntities.gc().SaveChanges();
		}
		private void Save_Click(object sender, RoutedEventArgs e)
		{ File.WriteAllText(FileManager.GetSensorsJson(), JsonConvert.SerializeObject(DB.Childs)); }
		private void SaveToExcelSensors_Click(object sender, RoutedEventArgs e)
		{
			var list = JsonConvert.DeserializeObject<List<SensorDetails>>(File.ReadAllText(FileManager.GetSensorsJson()));

			Excel.Application ExcelApp = new Excel.Application();
			Excel._Workbook ExcelWorkBook;
			Excel.Worksheet ExcelWorkSheet;
			ExcelWorkBook = ExcelApp.Workbooks.Open($@"{FileManager.GetAppData()}Отчет-Сенсор-Шаблон.xlsx");
			ExcelWorkSheet = (Excel.Worksheet)ExcelWorkBook.Worksheets.Item[1];

			// Заполнение ячеек
			// Установка даты в отчет
			ExcelWorkSheet.Cells[4, 2] = DateTime.Now.ToShortDateString() + " года";

			var field = dbMonitoringEntities.gc().Fields.Single(x => x.ID == DB.SelectSeeding.IDField);

			ExcelWorkSheet.Cells[8, 6] = field.District;
			ExcelWorkSheet.Cells[9, 7] = field.Number;
			ExcelWorkSheet.Cells[10, 5] = field.Position;

			ExcelWorkSheet.Cells[12, 4] = field.Number;
			ExcelWorkSheet.Cells[13, 4] = field.Position;

			foreach (var sensor in list)
			{
				ExcelWorkSheet.Cells[14, 4] = sensor.ID;

				var sensors = list.Where(x => x.ID == sensor.ID).ToList();
				for (int i = 0; i < sensors.Count(); i++)
				{
					SensorDetails _sensor = sensors[i];
					ExcelWorkSheet.Cells[17 + i, 3] = _sensor.Temperature;
					ExcelWorkSheet.Cells[17 + i, 4] = _sensor.Acidity;
					ExcelWorkSheet.Cells[17 + i, 5] = _sensor.Humidity;
					ExcelWorkSheet.Cells[17 + i, 6] = _sensor.Phosphorus;
					ExcelWorkSheet.Cells[17 + i, 7] = _sensor.Calium;
					ExcelWorkSheet.Cells[17 + i, 8] = _sensor.Magniy;
					ExcelWorkSheet.Cells[17 + i, 9] = _sensor.Calcium;
					ExcelWorkSheet.Cells[17 + i, 10] = _sensor.Asot;
					ExcelWorkSheet.Cells[17 + i, 11] = _sensor.Recomendation;
				}
				// Сохранение файла
				var settings = FileManager.GetSettings();
				ExcelApp.ActiveWorkbook.SaveAs($@"{settings.ReportsPath}\Отчет-Сенсор{sensor.ID}-{DateTime.Now.ToShortDateString()}.xlsx");
				// Закрытие процесса Excel
				var etc = Process.GetProcesses();
				foreach (var anti in etc)
					if (anti.ProcessName.ToLower().Contains("excel")) anti.Kill();
				mb.Show($@"Отчет по датчику №{sensor.ID} успешно сохранен");
			}
		}
		private int GetPercent(SensorDetails sensor)
		{
			int max = 0;
			var culture = dbMonitoringEntities.gc().Cultures.Where(x => x.Name == CBCulture.SelectedItem.ToString() && x.Status == Status.Content.ToString()).ToList().Single();

			double acidityMiddle = (culture.Ph.Contains('-')) ? (double.Parse(culture.Ph.Split('-')[0]) + double.Parse(culture.Ph.Split('-')[1])) / 2 : double.Parse(culture.Ph);
			double acidityPercent = Math.Floor(Convert.ToDouble(sensor.Acidity) / acidityMiddle * 100);

			double asotMiddle = (culture.Nitrogen.Contains('-')) ? (double.Parse(culture.Nitrogen.Split('-')[0]) + double.Parse(culture.Nitrogen.Split('-')[1])) / 2 : double.Parse(culture.Nitrogen);
			var asotPercent = Math.Floor(double.Parse(sensor.Asot) / asotMiddle * 100);

			double calciumMiddle = (culture.Calcium.Contains('-')) ? (double.Parse(culture.Calcium.Split('-')[0]) + double.Parse(culture.Calcium.Split('-')[1])) / 2 : double.Parse(culture.Calcium);
			var calciumPercent = Math.Floor(double.Parse(sensor.Calcium) / calciumMiddle * 100);

			double caliumMiddle = (culture.Potassium.Contains('-')) ? (double.Parse(culture.Potassium.Split('-')[0]) + double.Parse(culture.Potassium.Split('-')[1])) / 2 : double.Parse(culture.Potassium);
			var caliumPercent = Math.Floor(double.Parse(sensor.Calium) / caliumMiddle * 100);

			double humidityMiddle = (culture.Humidity.Contains('-')) ? (double.Parse(culture.Humidity.Split('-')[0]) + double.Parse(culture.Humidity.Split('-')[1])) / 2 : double.Parse(culture.Humidity);
			var humidityPercent = Math.Floor(double.Parse(sensor.Humidity) / humidityMiddle * 100);

			double magniyMiddle = (culture.Magnesium.Contains('-')) ? (double.Parse(culture.Magnesium.Split('-')[0]) + double.Parse(culture.Magnesium.Split('-')[1])) / 2 : double.Parse(culture.Magnesium);
			var magniyPercent = Math.Floor(double.Parse(sensor.Magniy) / magniyMiddle * 100);

			double phosphorMiddle = (culture.Phosphor.Contains('-')) ? (double.Parse(culture.Phosphor.Split('-')[0]) + double.Parse(culture.Phosphor.Split('-')[1])) / 2 : double.Parse(culture.Phosphor);
			var phosphorPercent = Math.Floor(double.Parse(sensor.Phosphorus) / phosphorMiddle * 100);

			double temperatureMiddle = (culture.Temperature.Contains('-')) ? (double.Parse(culture.Temperature.Split('-')[0]) + double.Parse(culture.Temperature.Split('-')[1])) / 2 : double.Parse(culture.Temperature);
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