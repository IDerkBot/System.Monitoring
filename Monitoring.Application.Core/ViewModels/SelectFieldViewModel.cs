using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Monitoring.Models.Entity;
using SystemMonitoringNetCore.Infrastructure.Command;
using SystemMonitoringNetCore.Models;
using SystemMonitoringNetCore.ViewModels.Base;
using SystemMonitoringNetCore.Views.Pages;
using SystemMonitoringNetCore.Views.Pages.EditPages;

namespace SystemMonitoringNetCore.ViewModels;

public class SelectFieldViewModel : BaseViewModel
{
    #region Переменные

    #region Districts : ObservableCollection<District> - Список районов

    private ObservableCollection<District> _districts;

    /// <summary> Список районов </summary>
    public ObservableCollection<District> Districts
    {
        get => _districts;
        set => SetField(ref _districts, value);
    }

    #endregion Districts

    #region SelectedDistrict : District? - Выбранный район

    private District? _selectedDistrict;

    /// <summary> Выбранный район </summary>
    public District? SelectedDistrict
    {
        get => _selectedDistrict;
        set
        {
            if (SetField(ref _selectedDistrict, value))
            {
                UpdateFieldsCollection();
                CanSelectField = _selectedDistrict != null;
            }
        }
    }

    #endregion SelectedDistrict

    #region Fields : ObservableCollection<Field> - Список полей

    private ObservableCollection<Field> _fields;

    /// <summary> Список полей </summary>
    public ObservableCollection<Field> Fields
    {
        get => _fields;
        set => SetField(ref _fields, value);
    }

    #endregion Fields

    #region SelectedField : Field? - Выбранное поле

    private Field? _selectedField;

    /// <summary> Выбранное поле </summary>
    public Field? SelectedField
    {
        get => _selectedField;
        set => SetField(ref _selectedField, value);
    }

    #endregion SelectedField

    #region CanSelectField : bool - Указывает на то, можем ли мы выбирать поле

    private bool _canSelectField;

    /// <summary> Указывает на то, можем ли мы выбирать поле </summary>
    public bool CanSelectField
    {
        get => _canSelectField;
        set => SetField(ref _canSelectField, value);
    }

    #endregion CanSelectField

    #endregion

    #region Комманды

    #region Next - Перемещение на окно информации о поле

    /// <summary> Перемещение на окно информации о поле </summary>
    public ICommand NextCommand { get; }

    private void OnNextCommandExecute(object parameter)
    {
        // создаем переменную seed, которую в дальнейшем будем передавать
        var seed = new Seed { Field = SelectedField };

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

    private bool CanNextCommandExecuted(object parameter) => SelectedDistrict != null && SelectedField != null;

    #endregion Next

    #region AddDistrict - Добавляем новый район

    /// <summary> Добавляем новый район </summary>
    public ICommand AddDistrictCommand { get; }

    private void OnAddDistrictCommandExecute(object parameter) => ManagerPage.Navigate(new AddDistrict());

    private bool CanAddDistrictCommandExecuted(object parameter) => true;

    #endregion AddDistrict

    #region AddField - Добавляем новое поле для выбраного района

    /// <summary> Добавляем новое поле для выбраного района </summary>
    public ICommand AddFieldCommand { get; }

    private void OnAddFieldCommandExecute(object parameter)
    {
        if (SelectedDistrict != null) ManagerPage.Navigate(new AddField(SelectedDistrict));
    }

    private bool CanAddFieldCommandExecuted(object parameter) => SelectedDistrict != null;

    #endregion AddField

    #endregion

    public SelectFieldViewModel()
    {
        #region Комманды

        NextCommand = new RelayCommand(OnNextCommandExecute, CanNextCommandExecuted);
        AddDistrictCommand = new RelayCommand(OnAddDistrictCommandExecute, CanAddDistrictCommandExecuted);
        AddFieldCommand = new RelayCommand(OnAddFieldCommandExecute, CanAddFieldCommandExecuted);

        #endregion

        Districts = new ObservableCollection<District>(Db.DbContext.Districts.ToList());
    }

    private void UpdateFieldsCollection()
    {
        if (Fields == null) Fields = new ObservableCollection<Field>();
        Fields.Clear();
        var fields = Db.DbContext.Fields.Where(x => x.District == SelectedDistrict).ToList();
        foreach (var field in fields) Fields.Add(field);
    }
}