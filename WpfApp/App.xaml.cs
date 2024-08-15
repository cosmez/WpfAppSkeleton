using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Microsoft.Extensions.Configuration;
using WpfApp.Library;
using WpfApp.Library.Services;
using WpfApp.Models.ViewModels;
using System.IO;
using WpfApp.Models;
using WpfApp.Services;

namespace WpfApp;
/// <summary>
/// TODO:
///     - [ ] Add Comments
///     - [X] Implement Dialog Service
///     - [ ] Add UI example with cancellation
///     - [ ] Add Pdfium Render example
///     - [ ] Add AvalonEdit example
///     - [ ] Add Tree Control example
///     - [ ] Add Markdown rendering example
///     - [ ] Look for more examples
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

    /// <summary>
    /// Shotcut to get a dependency
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
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
        services.AddTransient<IDialogService, DialogService>();


        //ViewModels
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<GridPageViewModel>();
        services.AddTransient<DialogsPageViewModel>();
        services.AddTransient<TaskCancellationViewModel>();


        //Main navigation items and view model creator
        var items = new NavigationItems();
        items.Add(new NavigationItem() { Description = "Grid Example", Page = NavigationPage.Grid, GetViewModel = GetService<GridPageViewModel> });
        items.Add(new NavigationItem() { Description = "DialogService Example", Page = NavigationPage.Dialogs, GetViewModel = GetService<DialogsPageViewModel> });
        items.Add(new NavigationItem() { Description = "Task Cancellation Example", Page = NavigationPage.Tasks, GetViewModel = GetService<TaskCancellationViewModel> });
        services.AddSingleton(items);

        return services.BuildServiceProvider();
        
    }

}

