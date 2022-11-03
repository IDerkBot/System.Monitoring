using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Monitoring.Models.Entity;
using SystemMonitoringNetCore.Models;

namespace SystemMonitoringNetCore.Views.Pages;

/// <summary>
/// Логика взаимодействия для FieldSelectPage.xaml
/// </summary>
public partial class FieldSelectPage
{
    #region OnLoad

    /// <summary>
    /// 
    /// </summary>
    public FieldSelectPage() => InitializeComponent();

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        // Загружаем районы
        CbDistrict.ItemsSource = Db.DbContext.Fields.ToList()
            .GroupBy(x => x.District).Select(x => x.Key).ToList();
        
        // Если район не был выбран до этого
        if (string.IsNullOrWhiteSpace(Db.DistrictName)) return;
        
        // Если выбран
        List<string?> districts = Db.DbContext.Fields.Select(x => x.District).ToList();
        districts.Add(Db.DistrictName);
        CbDistrict.ItemsSource = districts;
        CbDistrict.SelectedItem = Db.DistrictName;
        var fields = Db.DbContext.Fields
            .Where(x => x.District == Db.DistrictName).ToList();
        if (fields.Any())
            CbField.ItemsSource = fields;
    }

    #endregion

    #region Select Change

    private void DistrictSelectChanged(object sender, SelectionChangedEventArgs e)
    {
        SpFieldNumber.IsEnabled = true;
        var selectDistrict = CbDistrict.SelectedValue.ToString();
        var fields = Db.DbContext.Fields.ToList();
        CbField.ItemsSource = fields.Where(x => x.District == selectDistrict);
    }

    private void FieldDistrictChanged(object sender, SelectionChangedEventArgs e)
    {
        BtnNext.IsEnabled = true;
    }

    #endregion

    #region Buttons Event

    // Нажатие на кнопку вперед
    private void BtnNext_OnClick(object sender, RoutedEventArgs e)
    {
        // Заносим в переменную field выбранный элемент, который приводим к классу Field
        var field = CbField.SelectedItem as Field;

        // создаем переменную seed, которую в дальнейшем будем передавать
        var seed = new Seed { Field = field };

        // проверяем, есть ли в таблице seed элементы, которые содержат выбранное поле
        if (Db.DbContext.Seeds.Any(x => x.Field == seed.Field))
        {
            // есть - заносим в локальную переменную
            seed = Db.DbContext.Seeds.Single(x => x.Field == seed.Field);
        }
        else
        {
            seed.Date = DateTime.Now;
            // нет - добавляем новый в бд
            Db.DbContext.Seeds.Add(seed);
            Db.DbContext.SaveChanges();
            // И получаем его после занесения
            seed = Db.DbContext.Seeds.Single(x => x.Field == seed.Field);
        }

        // Его мы сохраняем как глобальную переменную
        Db.SelectSeeding = seed;
        ManagerPage.FieldMonitoringPage = new FieldMonitoringPage(seed);
        // и переходим на страницу мониторинга
        ManagerPage.Navigate(new FieldMonitoringPage(seed));
    }

    private void AddDistrict_Click(object sender, RoutedEventArgs e)
    {
        ManagerPage.Navigate(new AddDistrict());
    }

    private void AddField_Click(object sender, RoutedEventArgs e)
    {
        Db.DistrictName = CbDistrict.SelectedItem.ToString();
        ManagerPage.Navigate(new AddField(CbDistrict.SelectedItem.ToString()));
    }

    #endregion
}