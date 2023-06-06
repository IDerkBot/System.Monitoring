namespace Monitoring.Models;

public sealed class WordViewModel : FileEntityViewModel
{
    public WordViewModel(string name) : base(name) { }

    public WordViewModel(FileSystemInfo fileInfo) : base(fileInfo) { }
}