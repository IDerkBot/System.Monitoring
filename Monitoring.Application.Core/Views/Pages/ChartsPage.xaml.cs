using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Controls;
using Monitoring.Models.Entity;
using OxyPlot;
using OxyPlot.Legends;
using OxyPlot.Series;
using Sensor = SystemMonitoringNetCore.Models.Sensor;

namespace SystemMonitoringNetCore.Views.Pages;

public partial class ChartsPage : Page
{
    private PlotModel Model { get; set; }
    
    public ChartsPage(List<Sensor> list)
    {
        InitializeComponent();

        var modelTemp = new PlotModel { Title = "Температура" };
        var tempPoints = list.Select((t, i) => new DataPoint(i, (double)t.Temperature)).ToList();
        var tempSeries = new LineSeries { ItemsSource = tempPoints, Title = "Температура" };
        var tempLegend = new Legend { GroupNameFont = "Температура" };
        
        modelTemp.Series.Add(tempSeries);
        modelTemp.Legends.Add(tempLegend);
        oxyTemp.Model = modelTemp;
        
        var temp2Points = list.Select((t, i) => new DataPoint(i, (double)t.Temperature)).ToList();
        var temp2Series = new LineSeries { ItemsSource = temp2Points, Title = "Температура" };
        var temp2Legend = new Legend { Font = "Температура" };
        
        var modelHumidity = new PlotModel { Title = "Влажность" };
        var humidityPoints = list.Select((t, i) => new DataPoint(i, (double)t.Humidity)).ToList();
        var humiditySeries = new LineSeries { ItemsSource = humidityPoints, Title = "Влажность", Color = OxyColors.Peru };
        var humidityLegend = new Legend { GroupNameFont = "Влажность" };
        var humidity2Series = new LineSeries { ItemsSource = humidityPoints, Title = "Влажность" };
        var humidity2Legend = new Legend { GroupNameFont = "Влажность" };
        modelHumidity.Series.Add(humiditySeries);
        modelHumidity.Legends.Add(humidityLegend);
        oxyHumidity.Model = modelHumidity;

        var modelAcidity = new PlotModel { Title = "Кислотность" };
        var acidityPoints = list.Select((t, i) => new DataPoint(i, (double)t.Acidity)).ToList();
        var aciditySeries = new LineSeries { ItemsSource = acidityPoints, Title = "Кислотность", Color = OxyColors.Brown };
        var acidityLegend = new Legend { GroupNameFont = "Кислотность" };
        var acidity2Series = new LineSeries { ItemsSource = acidityPoints, Title = "Кислотность" };
        var acidity2Legend = new Legend { GroupNameFont = "Кислотность" };
        modelAcidity.Series.Add(aciditySeries);
        modelAcidity.Legends.Add(acidityLegend);
        oxyAcidity.Model = modelAcidity;
        
        var modelPhosphorus = new PlotModel { Title = "Фосфор" };
        var phosphorusPoints = list.Select((t, i) => new DataPoint(i, (double)t.Phosphorus)).ToList();
        var phosphorusSeries = new LineSeries { ItemsSource = phosphorusPoints, Title = "Фосфор", Color = OxyColors.Blue };
        var phosphorusLegend = new Legend { GroupNameFont = "Фосфор" };
        var phosphorus2Series = new LineSeries { ItemsSource = phosphorusPoints, Title = "Фосфор" };
        var phosphorus2Legend = new Legend { GroupNameFont = "Фосфор" };
        modelPhosphorus.Series.Add(phosphorusSeries);
        modelPhosphorus.Legends.Add(phosphorusLegend);
        oxyPhosphorus.Model = modelPhosphorus;
        
        var modelCalcium = new PlotModel { Title = "Кальций" };
        var calciumPoints = list.Select((t, i) => new DataPoint(i, (double)t.Calcium)).ToList();
        var calciumSeries = new LineSeries { ItemsSource = calciumPoints, Title = "Кальций", Color = OxyColors.Red };
        var calciumLegend = new Legend { GroupNameFont = "Кальций" };
        var calcium2Series = new LineSeries { ItemsSource = calciumPoints, Title = "Кальций" };
        var calcium2Legend = new Legend { GroupNameFont = "Кальций" };
        modelCalcium.Series.Add(calciumSeries);
        modelCalcium.Legends.Add(calciumLegend);
        oxyCalcium.Model = modelCalcium;
        
        var modelMagnesium = new PlotModel { Title = "Магнезий" };
        var magnesiumPoints = list.Select((t, i) => new DataPoint(i, (double)t.Magnesium)).ToList();
        var magnesiumSeries = new LineSeries { ItemsSource = magnesiumPoints, Title = "Магнезий", Color = OxyColors.Orange };
        var magnesiumLegend = new Legend { GroupNameFont = "Магнезий" };
        var magnesium2Series = new LineSeries { ItemsSource = magnesiumPoints, Title = "Магнезий" };
        var magnesium2Legend = new Legend { GroupNameFont = "Магнезий" };
        modelMagnesium.Series.Add(magnesiumSeries);
        modelMagnesium.Legends.Add(magnesiumLegend);
        oxyMagnesium.Model = modelMagnesium;
        
        var modelMain = new PlotModel { Title = "Все показатели" };
        modelMain.Series.Add(temp2Series);
        modelMain.Series.Add(humidity2Series);
        modelMain.Series.Add(acidity2Series);
        modelMain.Series.Add(phosphorus2Series);
        modelMain.Series.Add(calcium2Series);
        modelMain.Series.Add(magnesium2Series);
        
        modelMain.Legends.Add(temp2Legend);
        modelMain.Legends.Add(humidity2Legend);
        modelMain.Legends.Add(acidity2Legend);
        modelMain.Legends.Add(phosphorus2Legend);
        modelMain.Legends.Add(calcium2Legend);
        modelMain.Legends.Add(magnesium2Legend);
        
        OxyPlotView.Model = modelMain;
    }
}