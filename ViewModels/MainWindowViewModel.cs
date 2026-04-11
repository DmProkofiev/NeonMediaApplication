using Microsoft.Win32;
using NeonMediaApplication.Interfaces;
using NeonMediaApplication.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace NeonMediaApplication.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        //Хранение Обьектов
        private readonly IMediaEngine _mediaEngine; //обьект движка VLC
        private readonly IFileService _fileService; // интерфейс файлового сервиса

        public bool isTurningOn = false; //Статус воспроизведения
        private ObservableCollection<MediaFile> PlayList { get; set; } = new ObservableCollection<MediaFile>(); //Коллекция для хранения плейлиста
        public MainWindowViewModel(IMediaEngine mediaEngine, IFileService fileService) //Конструктор DI
        {
            _mediaEngine = mediaEngine;
            _fileService = fileService;
        }
        private string _file;
        public string File
        {
            get => _file;
            set
            {
                _file = value;
                OnPropertyChanged(nameof(File));
            }
        }
        private RelayCommand _openFileCommand;
        public RelayCommand OpenFileCommand
        {
            get
            {
                return _openFileCommand ?? (_openFileCommand = new RelayCommand(async obj =>
                {
                    var dialog = new OpenFileDialog();
                    dialog.Filter = "Медиафайлы|*.mp3;*.flac;*.wav;*.mp4;*.mkv;*.avi|Все файлы|*.*";
                    if(_fileService == null)
                    {
                        MessageBox.Show("Не инициализировано");
                    }

                    if (dialog.ShowDialog() == true)
                    {
                        foreach (string filePath in dialog.FileNames)
                        {
                            try
                            {
                                var mediaFile = await _fileService.ParseMediaAsync(filePath);
                                PlayList.Add(mediaFile);
                                //MessageBox.Show($"{mediaFile.FileName} {mediaFile.FilePath} {mediaFile.Duration} {mediaFile.Type} {mediaFile}"); //отладка парсинга файла
                            }
                            catch (NotSupportedException ex)
                            {
                                MessageBox.Show($"Файл не поддерживается: {filePath}\n{ex.Message}", "Ошибка");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Ошибка при открытии файла: {filePath}\n{ex.Message}", "Ошибка");
                            }
                        }
                    } 
                }));
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
