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
    }
}
