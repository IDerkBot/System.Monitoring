using System.Windows.Controls;
using System.Windows.Media;

namespace SystemMonitoring.Models
{
    internal static class Validation
	{
		public static void ChangeColor(this TextBox tb, SolidColorBrush background, SolidColorBrush foreground)
		{
			tb.BorderBrush = background;
			tb.Foreground = foreground;
		}
		public static void ChangeColor(this PasswordBox pb, SolidColorBrush background, SolidColorBrush foreground)
		{
			pb.BorderBrush = background;
			pb.Foreground = foreground;
		}
	}
}
