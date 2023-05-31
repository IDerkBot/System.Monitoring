using System.Diagnostics;
using System.Threading;
using System.Windows.Input;
using Monitoring.Models;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;

namespace SystemMonitoringNetCore.Infrastructure.UserControls;

public partial class FileEntityView
{
    public FileEntityView()
    {
        InitializeComponent();
    }

    private void FileEntityView_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (DataContext is FileEntityViewModel file)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = $"/c \"{file.FullName}\""
            };
            process.StartInfo = startInfo;
            process.Start();
            
            Thread.Sleep(1000);
            
            process.Kill();
        }
    }
}