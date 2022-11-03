using System.Windows.Controls;
using SystemMonitoring.Views.Pages;

namespace SystemMonitoring.Models
{
    /// <summary>
	/// Менеджер страниц
	/// </summary>
	internal class ManagerPage
	{

		internal static Frame Page { get; set; }
		internal static FieldMonitoring FieldMonitoringPage { get; set; }

		/// <summary>
		/// Перемещение на страницу
		/// </summary>
		/// <param name="page">Страница на которую требуется перейти</param>
		internal static void Navigate(Page page)
		{
			Page.Navigate(page);
		}
        /// <summary>
		/// Возвращение на страницу назад
		/// </summary>
		internal static void Back() => Page.GoBack();
	}
}