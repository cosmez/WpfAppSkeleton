﻿using System.ComponentModel;
using System.Windows;
using WpfApp.Models.ViewModels;

namespace WpfApp
{
    /// <summary>
    /// ViewModelLocator, used by the views to locate the ViewModel in runtime and design time
    /// </summary>
    public class ViewModelLocator
    {
        private readonly DependencyObject _dummy = new();
        
        //Include your view models here
        public MainWindowViewModel MainViewModel => GetViewModel<MainWindowViewModel>();
        public GridPageViewModel GridPageViewModel => GetViewModel<GridPageViewModel>();
        public DialogsPageViewModel FormPageViewModel => GetViewModel<DialogsPageViewModel>();
        public TaskCancellationViewModel TaskPageViewModel => GetViewModel<TaskCancellationViewModel>();
        public PdfPageViewModel PdfPageViewModel => GetViewModel<PdfPageViewModel>();
        public MarkdownEditorViewModel MarkdownEditorViewModel => GetViewModel<MarkdownEditorViewModel>();


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
