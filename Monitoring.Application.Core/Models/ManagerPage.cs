using System.Windows.Controls;
using SystemMonitoringNetCore.Views.Pages;

namespace SystemMonitoringNetCore.Models;

/// <summary>
/// Менеджер страниц
/// </summary>
internal class ManagerPage
{
    internal static Frame Page { get; set; }
    internal static FieldMonitoringPage FieldMonitoringPage { get; set; }

    /// <summary>
    /// Перемещение на страницу
    /// </summary>
    /// <param name="page">Страница на которую требуется перейти</param>
    internal static void Navigate(object page)
    {
        Page.Navigate(page);
    }

    /// <summary>
    /// Возвращение на страницу назад
    /// </summary>
    internal static void Back() => Page.GoBack();

    internal static void ClearHistory()
    {
        var navigationService = Page.NavigationService;
        while (navigationService.CanGoBack) navigationService.RemoveBackEntry();
    }

    public static bool CanGoBack() => Page.CanGoBack;
}