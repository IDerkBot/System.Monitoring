using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using SystemMonitoring.Models;
using static System.Windows.Media.Color;
using Excel = Microsoft.Office.Interop.Excel;
using Path = System.IO.Path;
using WinForms = System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace SystemMonitoring.AdminPages
{
    public partial class Reports : Page
    {
        public Reports() { InitializeComponent(); }
        private void FileSearch(string path)
        {
            //Уничтожить все объекты
            ExcelPanel.Children.Clear();
            WordPanel.Children.Clear();
            DB.Path = path;
            var filesExcel = Directory.GetFiles(path);
            var filesWord = Directory.GetFiles(path);
            var listExcel = (from file in filesExcel where Path.GetFileName(file).Contains("xlsm") || Path.GetFileName(file).Contains("xlsx") select Path.GetFileName(file)).ToList();
            var listWord = (from file in filesWord where Path.GetFileName(file).Contains("doc") || Path.GetFileName(file).Contains("docx") select Path.GetFileName(file)).ToList();
            //Excel
            if (listExcel.Count > 0)
            {
	            foreach (var file in listExcel)
	            {
		            var stack = new StackPanel { Orientation = Orientation.Horizontal };
		            var label = new Label
		            {
			            Content = $"{file}",
			            Width = WindowWidth / 2 - 105,
			            Foreground = new SolidColorBrush(FromRgb(0, 0, 0)),
			            DataContext = file
		            };
		            label.MouseDoubleClick += OpenFile;
		            var btn = new Button
		            {
			            Content = "Открыть",
			            DataContext = file
		            };
		            btn.Click += OpenFile;
		            stack.Children.Add(label);
		            stack.Children.Add(btn);
		            ExcelPanel.Children.Add(stack);
	            }
            }
            //Word
            if(listWord.Count > 0)
            {
                for (int i = 0; i < listWord.Count; i++)
                {
                    StackPanel stack = new StackPanel { Orientation = Orientation.Horizontal };
                    Label label = new Label
                    {
                        Content = $"{listWord[i]}",
                        Width = WindowWidth / 2 - 105,
                        Foreground = new SolidColorBrush(FromRgb(255, 255, 255)),
                        DataContext = $"{listWord[i]}"
                    };
                    label.MouseDoubleClick += new MouseButtonEventHandler(OpenFile);
                    Button btn = new Button
                    {
                        Content = "Открыть",
                        Width = 100,
                        DataContext = $"{listWord[i]}"
                    };
                    btn.Click += new RoutedEventHandler(OpenFile);
                    stack.Children.Add(label);
                    stack.Children.Add(btn);
                    WordPanel.Children.Add(stack);
                }
            }
        }
        private void OpenFile(object sender, EventArgs e)
        {
            string fileName = (sender.ToString().Contains("Button")) ? $"{(sender as Button).DataContext}" : $"{(sender as Label).DataContext}";
            string filePath = $@"{DB.Path}{fileName}";
            if (fileName.Contains("xlsm") || fileName.Contains("xlsx"))
            {
                Excel.Application exApp = new Excel.Application { Visible = true };
                exApp.Workbooks.Open(filePath);
            }
            else if (fileName.Contains("doc") || fileName.Contains("docx"))
            {
                Word.Application wdApp = new Word.Application { Visible = true };
                wdApp.Documents.Open(filePath);
            }
        }
        private void Research_Click(object sender, RoutedEventArgs e)
        {
            WinForms.FolderBrowserDialog folderDialog = new WinForms.FolderBrowserDialog { ShowNewFolderButton = false };
            if (folderDialog.ShowDialog() == WinForms.DialogResult.OK) FilePath.Text = folderDialog.SelectedPath;
            FileSearch($@"{FilePath.Text}\");
            CheckChangePath();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ((Page) sender).Width = WindowWidth;
            FilePath.Text = GetReportsPath();
        }
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e) { (sender as Page).Width = WindowWidth; }
        private void FilePath_TextChanged(object sender, TextChangedEventArgs e) { FileSearch($@"{FilePath.Text}\"); }
        private string GetReportsPath() { return FileManager.GetSettings().ReportsPath; }
        private void CheckChangePath()
        {
            if (GetReportsPath() != FilePath.Text)
            {
                Settings settings = FileManager.GetSettings();
                settings.ReportsPath = @FilePath.Text;
                FileManager.SetSettings(settings);
            }
        }

        private void BtnOpenExcel_OnClick(object sender, RoutedEventArgs e)
        {
	        
        }

        private void BtnOpenWord_OnClick(object sender, RoutedEventArgs e)
        {
	        
        }
    }
}