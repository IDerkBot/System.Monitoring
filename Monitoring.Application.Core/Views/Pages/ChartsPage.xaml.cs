using System.Collections.Generic;
using System.Linq;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.VisualElements;
// using OxyPlot;
// using OxyPlot.Legends;
// using OxyPlot.Series;
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

        ISeries[] seriesTemp =
        {
            new LineSeries<double>
            {
                Values = list.Select(x => (double)x.Temperature).ToList(),
                Fill = null
            }
        };
        ISeries[] seriesHumidity =
        {
            new LineSeries<double>
            {
                Values = list.Select(x => (double)x.Humidity).ToList(),
                Fill = null
            }
        };
        ISeries[] seriesAcidity =
        {
            new LineSeries<double>
            {
                Values = list.Select(x => (double)x.Acidity).ToList(),
                Fill = null
            }
        };
        ISeries[] seriesPhosphorus =
        {
            new LineSeries<double>
            {
                Values = list.Select(x => (double)x.Phosphorus).ToList(),
                Fill = null
            }
        };
        ISeries[] seriesCalcium =
        {
            new LineSeries<double>
            {
                Values = list.Select(x => (double)x.Calcium).ToList(),
                Fill = null
            }
        };
        ISeries[] seriesMagnesium =
        {
            new LineSeries<double>
            {
                Values = list.Select(x => (double)x.Magnesium).ToList(),
                Fill = null
            }
        };
        ISeries[] seriesGeneral =
        {
            new LineSeries<double>
            {
                Values = list.Select(x => (double)x.Temperature).ToList(),
                Fill = null
            },
            new LineSeries<double>
            {
                Values = list.Select(x => (double)x.Humidity).ToList(),
                Fill = null
            },
            new LineSeries<double>
            {
                Values = list.Select(x => (double)x.Acidity).ToList(),
                Fill = null
            },
            new LineSeries<double>
            {
                Values = list.Select(x => (double)x.Phosphorus).ToList(),
                Fill = null
            },
            new LineSeries<double>
            {
                Values = list.Select(x => (double)x.Calcium).ToList(),
                Fill = null
            },
            new LineSeries<double>
            {
                Values = list.Select(x => (double)x.Magnesium).ToList(),
                Fill = null
            }
        };

        LvTemp.Series = seriesTemp;
        // LvTemp.Title = new LabelVisual { Text = "Температура" };
        LvHumidity.Series = seriesHumidity;
        LvAcidity.Series = seriesAcidity;
        LvPhosphorus.Series = seriesPhosphorus;
        LvCalcium.Series = seriesCalcium;
        LvMagnesium.Series = seriesMagnesium;
        LvGeneral.Series = seriesGeneral;
        
        #endregion
    }
}