using System.Collections.Generic;
using System.Linq;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Sensor = SystemMonitoringNetCore.Models.Sensor;

namespace SystemMonitoringNetCore.Views.Pages;

/// <summary>  </summary>
public partial class ChartsPage
{
    /// <summary>  </summary>
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
        ISeries[] seriesSalinity =
        {
            new LineSeries<double>
            {
                Values = list.Select(x => (double)x.Salinity).ToList(),
                Fill = null
            }
        };
        ISeries[] seriesSodium =
        {
            new LineSeries<double>
            {
                Values = list.Select(x => (double)x.Sodium).ToList(),
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
                Values = list.Select(x => (double)x.Salinity).ToList(),
                Fill = null
            },
            new LineSeries<double>
            {
                Values = list.Select(x => (double)x.Sodium).ToList(),
                Fill = null
            }
        };

        LvTemp.Series = seriesTemp;
        // LvTemp.Title = new LabelVisual { Text = "Температура" };
        LvHumidity.Series = seriesHumidity;
        LvAcidity.Series = seriesAcidity;
        LvPhosphorus.Series = seriesPhosphorus;
        LvCalcium.Series = seriesSalinity;
        LvMagnesium.Series = seriesSodium;
        LvGeneral.Series = seriesGeneral;
        
        #endregion
    }
}