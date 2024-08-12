using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.Configuration;
using WpfApp.Library;
using WpfApp.Library.Services;
using WpfApp.Models.ViewModels;
using System.IO;

namespace WpfApp;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
    /// </summary>
    public IServiceProvider Services { get; }

    public IConfiguration Configuration { get; }

    /// <summary>
    /// Gets the current <see cref="App"/> instance in use
    /// </summary>
    public new static App Current => (App)Application.Current;

    public static T GetService<T>() where T : notnull => Current.Services.GetRequiredService<T>();

    public App()
    {
        Configuration = new ConfigurationBuilder()
            .AddUserSecrets<IWpfAppInterface>() // Add UserSecrets configuration
            .Build();
        
        Services = ConfigureServices(Configuration);
    }
    
    /// <summary>
    /// Configures the services for the application.
    /// </summary>
    private static IServiceProvider ConfigureServices(IConfiguration configuration)
    {

        var services = new ServiceCollection();

        //create main application folder
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string appPath = Path.Combine(appDataPath, nameof(WpfApp));
        if (!Directory.Exists(appPath)) Directory.CreateDirectory(appPath);

        var settings = new Settings()
        {
            Database = configuration["Database"] ?? "database.db",
            Storage = configuration["Storage"] ?? appPath
        };
        services.AddSingleton(settings);

        //Service Dependencies
        services.AddTransient<SampleService>();
        services.AddTransient<StorageService>();
        services.AddSingleton<DatabaseService>();



        //ViewModels
        services.AddTransient<MainWindowViewModel>();
        return services.BuildServiceProvider();
    }

}

