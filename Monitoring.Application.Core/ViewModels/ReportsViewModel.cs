using System.Windows.Input;
using Microsoft.Win32;
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
        set => SetField(ref _path, value);
    }

    #endregion Path

    #region Explorer - Открытие проводника

    /// <summary> Открытие проводника </summary>
    public ICommand ExplorerCommand { get; }

    private void OnExplorerCommandExecuted(object parameter)
    {
        // var ofd = new OpenFileDialog();
        // ofd.ShowDialog();
        
        
    }

    #endregion Expolorer
    
    public ReportsViewModel(IDialogService dialogService)
    {
        _dialogService = dialogService; 
        ExplorerCommand = new RelayCommand(OnExplorerCommandExecuted);
    }
}