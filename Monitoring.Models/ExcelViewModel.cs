namespace Monitoring.Models;

public sealed class ExcelViewModel : FileEntityViewModel
{
    public ExcelViewModel(string name) : base(name) { }

    public ExcelViewModel(FileSystemInfo fileInfo) : base(fileInfo) { }
}