namespace WpfApp.Library.Services;

/// <summary>
/// Storage Service with an injected example
/// 
/// </summary>
public class StorageService
{
    private readonly Settings _settings;

    public StorageService(Settings settings)
    {
        _settings = settings;
    }
}
