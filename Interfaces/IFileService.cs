using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeonMediaApplication.Models;

namespace NeonMediaApplication.Interfaces
{
    //Интерфейс сервиса FileService
    //Задача: 
    public interface IFileService
    {
         Task<MediaFile> ParseMediaAsync(string path);
    }
}
