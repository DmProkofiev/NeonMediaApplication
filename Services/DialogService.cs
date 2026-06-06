using Microsoft.Win32;
using NeonMediaApplication.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NeonMediaApplication.Services
{
    public class DialogService: IDialogService
    {
        public string[]? OpenFiles(string filter)
        {
            var dialog = new OpenFileDialog
            {
                Filter = filter,
                Multiselect = true 
            };

            return dialog.ShowDialog() == true ? dialog.FileNames : null;
        }
        public void ShowError(string message, string title = "Ошибка")
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public void ShowMessage(string message, string title = "Информация")
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public string? SaveFile(string filter, string fileName, string defaultExtension = "", string title = "Сохранить как")
        {
            var dialog = new SaveFileDialog
            {
                Filter = filter,
                DefaultExt = defaultExtension,
                Title = title,
                FileName = fileName
            };
            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }
    }
}
