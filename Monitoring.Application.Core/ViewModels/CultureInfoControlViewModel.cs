using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Win32;
using Monitoring.Models.Entity;
using SystemMonitoringNetCore.Infrastructure.Command;
using SystemMonitoringNetCore.Models;
using SystemMonitoringNetCore.ViewModels.Base;
using SystemMonitoringNetCore.Views.UserControls;

namespace SystemMonitoringNetCore.ViewModels;

public class CultureInfoControlViewModel : BaseViewModel
{
    #region Properties

    #region SelectedCulture : Culture - Выбранная культура

    private Culture _selectedCulture;

    /// <summary> Выбранная культура </summary>
    public Culture SelectedCulture
    {
        get => _selectedCulture;
        set => SetField(ref _selectedCulture, value);
    }

    #endregion SelectedCulture

    #region Statuses : ObservableCollection<CultureStatusInfoViewModel> - Список статусов культуры

    private ObservableCollection<CultureStatusInfoViewModel> _statuses;

    /// <summary> Список статусов культуры </summary>
    public ObservableCollection<CultureStatusInfoViewModel> Statuses
    {
        get => _statuses;
        set => SetField(ref _statuses, value);
    }

    #endregion Statuses

    #region SelectedStatus : CultureStatusInfoViewModel - Выбранный статус культуры

    private CultureStatusInfoViewModel _selectedStatus;

    /// <summary> Выбранный статус культуры </summary>
    public CultureStatusInfoViewModel SelectedStatus
    {
        get => _selectedStatus;
        set => SetField(ref _selectedStatus, value);
    }

    #endregion SelectedStatus

    #region IsEdit : bool - Новая культура, значит режим редактирования

    private bool _isEdit;

    /// <summary> Новая культура, значит режим редактирования </summary>
    public bool IsEdit
    {
        get => _isEdit;
        set => SetField(ref _isEdit, value);
    }

    #endregion IsEdit

    #endregion

    #region Commands

    #region OpenSelectImage - Открывает выбор изображения

    /// <summary> Открывает выбор изображения </summary>
    public ICommand OpenSelectImageCommand { get; }

    private void OnOpenSelectImageCommandExecuted(object parameter)
    {
        var ofd = new OpenFileDialog();
        ofd.ShowDialog();
        var filePath = ofd.FileName;
        var source = ImageManager.CroppedToBitmapImage(filePath);
        var bytes = ImageManager.CroppedToBytes(source);
        SelectedCulture.Image = bytes;
        OnPropertyChanged(nameof(SelectedCulture));
        Db.DbContext.SaveChanges();
    }

    #endregion OpenSelectImage

    #region AddStatus - Добавление нового статуса культуры

    /// <summary> Добавление нового статуса культуры </summary>
    public ICommand AddStatusCommand { get; }

    private void OnAddStatusCommandExecuted(object parameter)
    {
        var vm = new AddCultureStatusControlViewModel(new CultureStatus { Culture = SelectedCulture });
        ManagerPage.Navigate(new AddCultureStatusControl { DataContext = vm });
    }

    #endregion AddCulture

    #region EditStatus - Редактирует выбранный статус

    /// <summary> Редактирует выбранный статус </summary>
    public ICommand EditStatusCommand { get; }

    private void OnEditStatusCommandExecuted(object parameter)
    {
        if (parameter is CultureStatusInfoViewModel cultureStatusInfoViewModel)
        {
            var vm = new AddCultureStatusControlViewModel(cultureStatusInfoViewModel.Status);
            ManagerPage.Navigate(new AddCultureStatusControl { DataContext = vm });
        }
    }

    private bool CanEditStatusCommandExecute(object parameter) => parameter is CultureStatusInfoViewModel;

    #endregion EditStatus

    #region DeleteStatus - Удалить выбранный статус

    /// <summary> Удалить выбранный статус </summary>
    public ICommand DeleteStatusCommand { get; }

    private void OnDeleteStatusCommandExecuted(object parameter)
    {
        if (parameter is CultureStatusInfoViewModel cultureStatusInfoViewModel)
        {
            Db.DbContext.CultureStatuses.Remove(cultureStatusInfoViewModel.Status);
            Db.DbContext.SaveChanges();
            UpdateCollection();
        }
    }

    private bool CanDeleteStatusCommandExecute(object parameter) => parameter is CultureStatusInfoViewModel;

    #endregion DeleteStatus
    
    #region Loaded - Загрузка представления

    /// <summary> Загрузка представления </summary>
    public ICommand LoadedCommand { get; }

    private void OnLoadedCommandExecuted(object parameter)
    {
        UpdateCollection();
    }

    #endregion Loaded

    #region SaveChanges - Сохранить данные

    /// <summary> Сохранить данные </summary>
    public ICommand SaveChangesCommand { get; }

    private void OnSaveChangesCommandExecuted(object parameter)
    {
        if (SelectedCulture.Id == 0) Db.DbContext.Cultures.Add(SelectedCulture);
        Db.DbContext.SaveChanges();
        IsEdit = false;
    }

    private bool CanSaveChangesCommandExecute(object parameter) => !string.IsNullOrWhiteSpace(SelectedCulture.Name);

    #endregion SaveChanges
    
    #endregion

    #region Constructor

    public CultureInfoControlViewModel(Culture selectedCulture)
    {
        LoadedCommand = new RelayCommand(OnLoadedCommandExecuted);
        AddStatusCommand = new RelayCommand(OnAddStatusCommandExecuted);
        EditStatusCommand = new RelayCommand(OnEditStatusCommandExecuted, CanEditStatusCommandExecute);
        DeleteStatusCommand = new RelayCommand(OnDeleteStatusCommandExecuted, CanDeleteStatusCommandExecute);
        OpenSelectImageCommand = new RelayCommand(OnOpenSelectImageCommandExecuted);
        SaveChangesCommand = new RelayCommand(OnSaveChangesCommandExecuted, CanSaveChangesCommandExecute);
        
        SelectedCulture = selectedCulture;
        if (string.IsNullOrWhiteSpace(SelectedCulture.Name)) IsEdit = true;
    }

    #endregion

    #region Private Methods

    private void UpdateCollection()
    {
        Statuses = new ObservableCollection<CultureStatusInfoViewModel>();
        foreach (var cultureStatus in Db.DbContext.CultureStatuses.Where(x => x.Culture == SelectedCulture)) Statuses.Add(new CultureStatusInfoViewModel(cultureStatus));
    }

    #endregion
}