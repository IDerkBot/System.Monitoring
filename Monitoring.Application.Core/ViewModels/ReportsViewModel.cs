using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Microsoft.Win32;
using Monitoring.Models;
using MvvmDialogs;
using SystemMonitoringNetCore.Infrastructure.Command;
using SystemMonitoringNetCore.ViewModels.Base;

namespace SystemMonitoringNetCore.ViewModels;

public class ReportsViewModel : BaseViewModel
{
    private readonly IDialogService _dialogService;
    
    #region Path : string - Путь к файлам

    private string _path;

    /// <summary> Путь к файлам </summary>
    public string Path
    {
        get => _path;
        set
        {
            SetField(ref _path, value);
            GetWorldAndExcelFiles();
        }
    }

    #endregion Path

    #region Files : ObservableCollection<FileEntityViewModel> - Список файлов

    private ObservableCollection<FileEntityViewModel> _files;

    /// <summary> Список файлов </summary>
    public ObservableCollection<FileEntityViewModel> Files
    {
        get => _files;
        set => SetField(ref _files, value);
    }

    #endregion Files

    #region Explorer - Открытие проводника

    /// <summary> Открытие проводника </summary>
    public ICommand ExplorerCommand { get; }

    private void OnExplorerCommandExecuted(object parameter)
    {
        // var ofd = new OpenFileDialog();
        // ofd.ShowDialog();
        // Path = ofd.FileName;
        
        // TODO Открыть Folder Dialog для захвата пути
    }

    #endregion Expolorer
    
    public ReportsViewModel(IDialogService dialogService)
    {
        _dialogService = dialogService; 
        ExplorerCommand = new RelayCommand(OnExplorerCommandExecuted);

        Files = new ObservableCollection<FileEntityViewModel>();
    }
    
    private void GetWorldAndExcelFiles()
    {
        if (!Path.EndsWith("\\"))
        {
            Path += "\\";
            return;
        }
        if(!Directory.Exists(Path)) return;
        var files = Directory.GetFiles(Path);
        var listWords = files.Where(file => file.EndsWith("docx") || file.EndsWith("doc"));
        var listExcels = files.Where(file => file.EndsWith("xlsx"));

        Files.Clear();
        foreach (var wordFile in listWords)
            Files.Add(new WorldViewModel(new FileInfo(wordFile)));
        foreach (var excelFile in listExcels)
            Files.Add(new ExcelViewModel(new FileInfo(excelFile)));
    }
}