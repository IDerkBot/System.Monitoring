using System.Collections.Generic;
using System.Linq;
using OxyPlot;
using OxyPlot.Legends;
using OxyPlot.Series;
using Sensor = SystemMonitoringNetCore.Models.Sensor;

namespace SystemMonitoringNetCore.Views.Pages;

/// <summary>
/// 
/// </summary>
public partial class ChartsPage
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="list"></param>
    public ChartsPage(IReadOnlyCollection<Sensor> list)
    {
        InitializeComponent();
        
        #region Отдельные графики

        var listTemperature = list.Select((t, i) => new DataPoint(i, (double)t.Temperature)).ToList();
        OxyTemp.Model = CreateNewModel("Температура", listTemperature, OxyColors.Bisque);
        var listHumidity = list.Select((t, i) => new DataPoint(i, (double)t.Humidity)).ToList();
        OxyHumidity.Model = CreateNewModel("Влажность", listHumidity, OxyColors.Peru);
        var listAcidity = list.Select((t, i) => new DataPoint(i, (double)t.Acidity)).ToList();
        OxyAcidity.Model = CreateNewModel("Кислотность", listAcidity, OxyColors.Brown);
        var listPhosphorus = list.Select((t, i) => new DataPoint(i, (double)t.Phosphorus)).ToList();
        OxyPhosphorus.Model = CreateNewModel("Фосфор", listPhosphorus, OxyColors.Blue);
        var listCalcium = list.Select((t, i) => new DataPoint(i, (double)t.Calcium)).ToList();
        OxyCalcium.Model = CreateNewModel("Кальций", listCalcium, OxyColors.Red);
        var listMagnesium = list.Select((t, i) => new DataPoint(i, (double)t.Magnesium)).ToList();
        OxyMagnesium.Model = CreateNewModel("Магнезий", listMagnesium, OxyColors.Orange);

        #endregion

        #region Общий график

        var listTitle = new List<string> { "Температура", "Влажность", "Кислотность", "Фосфор", "Кальций", "Магнезий" };
        var listData = new List<List<DataPoint>>
        {
            list.Select((t, i) => new DataPoint(i, (double)t.Temperature)).ToList(),
            list.Select((t, i) => new DataPoint(i, (double)t.Humidity)).ToList(),
            list.Select((t, i) => new DataPoint(i, (double)t.Acidity)).ToList(),
            list.Select((t, i) => new DataPoint(i, (double)t.Phosphorus)).ToList(),
            list.Select((t, i) => new DataPoint(i, (double)t.Calcium)).ToList(),
            list.Select((t, i) => new DataPoint(i, (double)t.Magnesium)).ToList()
        };

        var listColors = new List<OxyColor> { OxyColors.Bisque, OxyColors.Peru, OxyColors.Brown, OxyColors.Blue, OxyColors.Red, OxyColors.Orange };
        OxyPlotView.Model = CreateNewModel("Все показатели", listTitle, listData, listColors);

        #endregion
    }

    private static PlotModel CreateNewModel(string title, IEnumerable<DataPoint> list, OxyColor color)
    {
        var model = new PlotModel
        {
            Title = title,
            Series = { new LineSeries { ItemsSource = list, Title = title, Color = color } }
        };
        return model;
    }
    
    private static PlotModel CreateNewModel(string title, IReadOnlyList<string> titles, IReadOnlyList<List<DataPoint>> lists, IReadOnlyList<OxyColor> colors)
    {
        var model = new PlotModel { Title = title };
        for (var i = 0; i < titles.Count; i++)
        {
            var modelSeries = new LineSeries { ItemsSource = lists[i], Title = titles[i], Color = colors[i] };
            var modelLegend = new Legend { GroupNameFont = titles[i] };
            model.Series.Add(modelSeries);
            model.Legends.Add(modelLegend);
        }
        
        return model;
    }
}