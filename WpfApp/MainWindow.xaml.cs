using System.Windows;
using WpfApp.Models.ViewModels;

namespace WpfApp;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext;
    
    public MainWindow()
    {
        InitializeComponent(); 

    }
}