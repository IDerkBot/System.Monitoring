using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Monitoring.Models;

public abstract class FileEntityViewModel : INotifyPropertyChanged
{
    #region Name : string - Имя файла

    private string _name;

    /// <summary> Имя файла </summary>
    public string Name
    {
        get => _name;
        set => SetField(ref _name, value);
    }

    #endregion Name

    #region FullName : string - Путь к файлу

    private string _fullName;

    /// <summary> Путь к файлу </summary>
    public string FullName
    {
        get => _fullName;
        set => SetField(ref _fullName, value);
    }

    #endregion FullName

    public FileEntityViewModel(string name)
    {
        Name = name;
    }

    public FileEntityViewModel(FileSystemInfo fileInfo) : this(fileInfo.Name)
    {
        FullName = fileInfo.FullName;
    }
    
    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    #endregion
}