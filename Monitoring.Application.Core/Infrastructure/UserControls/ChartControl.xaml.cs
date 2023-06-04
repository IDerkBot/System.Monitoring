using System.Windows;
using System.Windows.Controls;

namespace SystemMonitoringNetCore.Infrastructure.UserControls;

public partial class ChartControl : UserControl
{
    public static readonly DependencyProperty ShowSectionsProperty = DependencyProperty.Register(
        nameof(ShowSections), typeof(bool), typeof(ChartControl), new PropertyMetadata(true));

    public bool ShowSections
    {
        get => (bool)GetValue(ShowSectionsProperty);
        set
        {
            SetValue(ShowSectionsProperty, value);
            if (value == false)
                Chart.Sections = null!;
        }
    }
    
    public ChartControl()
    {
        InitializeComponent();
        
    }

    private void ChartControl_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (ShowSections == false) Chart.Sections = null;
    }
}