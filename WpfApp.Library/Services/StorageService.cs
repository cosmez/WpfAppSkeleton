namespace WpfApp.Library.Services;

/// <summary>
/// Storage Service with an injected example
/// the idea is to abstract the basic things we do with storage
/// </summary>
public class StorageService
{
    private readonly Settings _settings;

    public StorageService(Settings settings)
    {
        _settings = settings;
    }
    
    
}
