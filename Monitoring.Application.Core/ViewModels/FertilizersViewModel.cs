using System.Collections.ObjectModel;
using System.Windows.Input;
using Monitoring.Models.Entity;
using SystemMonitoringNetCore.Infrastructure.Command;
using SystemMonitoringNetCore.Models;
using SystemMonitoringNetCore.ViewModels.Base;

namespace SystemMonitoringNetCore.ViewModels;

public class FertilizersViewModel : BaseViewModel
{
    #region Fertilizers : ObservableCollection<Fertilizer> - Список удобрений

    private ObservableCollection<Fertilizer> _fertilizers;

    /// <summary> Список удобрений </summary>
    public ObservableCollection<Fertilizer> Fertilizers
    {
        get => _fertilizers;
        set => SetField(ref _fertilizers, value);
    }

    #endregion Fertilizers

    #region Loaded - Загрузка представления

    /// <summary> Загрузка представления </summary>
    public ICommand LoadedCommand { get; }

    private void OnLoadedCommandExecuted(object parameter)
    {
        Fertilizers.Clear();
        foreach (var fertilizer in Db.DbContext.Fertilizers)
            Fertilizers.Add(fertilizer);
    }

    private bool CanLoadedCommandExecute(object parameter)
    {
        return true;
    }

    #endregion Loaded
    
    public FertilizersViewModel()
    {
        LoadedCommand = new RelayCommand(OnLoadedCommandExecuted, CanLoadedCommandExecute);
        
        Fertilizers = new ObservableCollection<Fertilizer>();
    }
}