using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SystemMonitoringNetCore.Models;
using Excel = Microsoft.Office.Interop.Excel;
using FileInfo = SystemMonitoringNetCore.Models.FileInfo;
using Path = System.IO.Path;
// using WinForms = System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace SystemMonitoring.Pages
{
	public partial class Reports : Page
	{
		public Reports() { InitializeComponent(); }
		private void FileSearch(string path)
		{
			if (!Directory.Exists(path)) return;

			Db.Path = path;
			var files = Directory.GetFiles(path);
			var excelFiles = (from file in files where Path.GetFileName(file).Contains("xlsm") || Path.GetFileName(file).Contains("xlsx") select Path.GetFullPath(file)).ToList();
			var wordFiles = (from file in files where Path.GetFileName(file).Contains("doc") || Path.GetFileName(file).Contains("docx") select Path.GetFullPath(file)).ToList();
			//Excel
			if (excelFiles.Count > 0)
			{
				var listExcel = excelFiles.Select(file => new FileInfo { FileName = file.Split('\\').Last(), Path = file }).ToList();
				DgExcelFiles.ItemsSource = listExcel;
			}
			//Word
			if (wordFiles.Count <= 0) return;
			var listWord = wordFiles.Select(file => new FileInfo { FileName = file.Split('\\').Last(), Path = file }).ToList();
			DgWordFiles.ItemsSource = listWord;

		}
		private void Research_Click(object sender, RoutedEventArgs e)
		{
			// var folderDialog = new WinForms.FolderBrowserDialog { ShowNewFolderButton = false };
			// if (folderDialog.ShowDialog() == WinForms.DialogResult.OK) FilePath.Text = folderDialog.SelectedPath;
			FileSearch($@"{FilePath.Text}\");
			CheckChangePath();
		}
		private void Page_Loaded(object sender, RoutedEventArgs e) => FilePath.Text = GetReportsPath();
		private void FilePath_TextChanged(object sender, TextChangedEventArgs e) { FileSearch($@"{FilePath.Text}\"); }
		private static string GetReportsPath() => FileManager.GetSettings().ReportsPath;
		private void CheckChangePath()
		{
			if (GetReportsPath() == FilePath.Text) return;
			var settings = FileManager.GetSettings();
			settings.ReportsPath = @FilePath.Text;
			FileManager.SetSettings(settings);
		}
		private void BtnOpenExcel_OnClick(object sender, RoutedEventArgs e)
		{
			var exApp = new Excel.Application { Visible = true };
			exApp.Workbooks.Open(((sender as Button)?.DataContext as FileInfo)?.Path);
		}
		private void BtnOpenWord_OnClick(object sender, RoutedEventArgs e)
		{
			// var wdApp = new Word.Application { Visible = true };
			// wdApp.Documents.Open(((sender as Button)?.DataContext as FileInfo)?.Path);
		}
	}
}