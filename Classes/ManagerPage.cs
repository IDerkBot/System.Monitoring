using System.Windows.Controls;
using SystemMonitoring.Pages;

namespace SystemMonitoring.Classes
{
	internal class ManagerPage
	{
		internal static Frame Page { get; set; }
		internal static FieldMonitoring FieldMonitoringPage { get; set; }

		internal static void Navigate(Page page)
		{
			for (var i = 1d; i >= 0; i -= 0.1)
				Page.Opacity = i;
			Page.Navigate(page);
			for (var i = 0d; i < 1.1; i += 0.1)
				Page.Opacity = i;
		}
		internal static void Back() => Page.GoBack();
	}
}