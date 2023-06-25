using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MapControl;
using Monitoring.Models.Entity;
using SystemMonitoringNetCore.ViewModels.Base;
using SystemMonitoringNetCore.Views.Pages;

namespace SystemMonitoringNetCore.ViewModels;

public class MapControlViewModel : BaseViewModel
{
    #region Points : ObservableCollection<PointItem> - Коллекция точек (датчики)

    private ObservableCollection<PointItem> _points;

    /// <summary> Коллекция точек (датчики) </summary>
    public ObservableCollection<PointItem> Points
    {
        get => _points;
        set => SetField(ref _points, value);
    }

    #endregion Points

    public MapControlViewModel(List<Sensor> sensors)
    {
        var points = sensors.Select(x => new PointItem
        {
            Name = x.Uid.ToString(),
            Location = new Location(x.PositionX, x.PositionY)
        });
        Points = new ObservableCollection<PointItem>(points);
    }
}

public class PointItem
{
    public string Name { get; set; }

    public Location Location { get; set; }
}