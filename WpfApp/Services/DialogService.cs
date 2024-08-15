using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using Ookii.Dialogs.Wpf;
using WpfApp.Models;

namespace WpfApp.Services
{
    public class DialogService : IDialogService
    {
        public bool ShowConfirmationDialog(string message, string title)
        {
            var result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }

        public void ShowMessageDialog(string message, string title, MessageBoxImage icon)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, icon);
        }
        public void ShowMessageDialog(string message, string title) => ShowMessageDialog(message, title, MessageBoxImage.Exclamation);
        public void ShowMessageInformationDialog(string message, string title) => ShowMessageDialog(message, title, MessageBoxImage.Information);
        public void ShowMessageErrorDialog(string message, string title) => ShowMessageDialog(message, title, MessageBoxImage.Error);

        public string[]? ShowOpenFileDialog(string message, string title, string filter = "All files (*.*)|*.*", bool multiSelect = false)
        {
            var dialog = new VistaOpenFileDialog();
            dialog.Filter = filter;
            if (!VistaFileDialog.IsVistaFileDialogSupported)
            {
                MessageBox.Show(
                    "Because you are not using Windows Vista or later, the regular open file dialog will be used. Please use Windows Vista to see the new dialog.",
                    "Sample open file dialog");
                return null;
            }

            dialog.Multiselect = multiSelect;
            var result = dialog.ShowDialog();
            if (result ?? false)
            {
                return dialog.FileNames;
            }

            return null;
        }

        public string[]? ShowSaveFileDialog(string message, string title, string filter = "All files (*.*)|*.*")
        {
            var dialog = new VistaSaveFileDialog();
            dialog.Filter = filter;
            if (!VistaFileDialog.IsVistaFileDialogSupported)
            {
                MessageBox.Show(
                    "Because you are not using Windows Vista or later, the regular open file dialog will be used. Please use Windows Vista to see the new dialog.",
                    "Sample open file dialog");
                return null;
            }

            var result = dialog.ShowDialog();
            if (result ?? false)
            {
                return dialog.FileNames;
            }

            return null;
        }

        public string[]? ShowFolderDialog(string message, string title, bool multiSelect = false)
        {
            var dialog = new VistaFolderBrowserDialog();
            dialog.Multiselect = true;
            dialog.Description = "Please select a folder.";
            dialog.UseDescriptionForTitle = true; // This applies to the Vista style dialog only, not the old dialog.

            if (!VistaFolderBrowserDialog.IsVistaFolderDialogSupported)
            {
                MessageBox.Show("Because you are not using Windows Vista or later, the regular folder browser dialog will be used. Please use Windows Vista to see the new dialog.", "Sample folder browser dialog");
            }

            var result = dialog.ShowDialog();
            if (result ?? false)
            {
                var selectedPaths = dialog.SelectedPaths;
                if (selectedPaths.Length == 1)
                {
                    return selectedPaths;
                }
            }

            return null;
        }
    }
}
