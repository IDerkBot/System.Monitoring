namespace Monitoring.Models;

public sealed class WorldViewModel : FileEntityViewModel
{
    public WorldViewModel(string name) : base(name) { }

    public WorldViewModel(FileSystemInfo fileInfo) : base(fileInfo) { }
}