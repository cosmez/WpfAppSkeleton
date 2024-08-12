namespace WpfApp.Library.Services;
public class StorageService
{
    private readonly Settings _settings;

    public StorageService(Settings settings)
    {
        _settings = settings;
    }
}
