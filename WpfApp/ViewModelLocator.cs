using System.ComponentModel;
using System.Windows;
using WpfApp.Models.ViewModels;

namespace WpfApp
{
    public class ViewModelLocator
    {
        private readonly DependencyObject _dummy = new DependencyObject();
        
        public MainWindowViewModel MainViewModel
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new MainWindowViewModel();
                }

                return App.GetService<MainWindowViewModel>();
            }
        }

        public GridPageViewModel GridPageViewModel => GetViewModel<GridPageViewModel>();
        
        
        private T GetViewModel<T>() where T : notnull, new()
        {
            if (IsInDesignMode())
            {
                return new T();
            }

            return App.GetService<T>();
        }

        private bool IsInDesignMode()
        {
            return DesignerProperties.GetIsInDesignMode(_dummy);
        }
    }
}
