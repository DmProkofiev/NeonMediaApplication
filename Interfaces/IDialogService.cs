using NeonMediaApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonMediaApplication.Interfaces
{
    public interface IDialogService
    {
        string[] OpenFiles(string filter);
        void ShowError(string message, string title = "Ошибка");
        void ShowMessage(string message, string title = "Информация");
        string SaveFile(string filter, string defaultExtension = "", string title = "Сохранить как");
    }
}
