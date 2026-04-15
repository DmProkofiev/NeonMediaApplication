using Microsoft.Win32;
using NeonMediaApplication.Interfaces;
using NeonMediaApplication.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Documents;

namespace NeonMediaApplication.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        //Хранение Обьектов
        private readonly IMediaEngine _mediaEngine; //обьект движка VLC
        private readonly IFileService _fileService; // интерфейс файлового сервиса
        private readonly IDialogService _dialogService; // интерфейс диалога

        public bool isTurningOn = false; //Статус воспроизведения
        private ObservableCollection<MediaFile> PlayList { get; set; } = new ObservableCollection<MediaFile>(); //Коллекция для хранения плейлиста общая
        public MainWindowViewModel(IMediaEngine mediaEngine, IFileService fileService, IDialogService dialogService) //Конструктор DI
        {
            _mediaEngine = mediaEngine;
            _fileService = fileService;
            _dialogService = dialogService;
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

        private MediaFile? _currentMedia; // Текущий медиа файл
        public MediaFile? CurrentMedia
        {
            get => _currentMedia;
            set
            {
                _currentMedia = value;
                OnPropertyChanged();
            }
        }
        //Команды
        private RelayCommand _playCommand;
        public RelayCommand PlayCommand
        {
            get
            {
                return _playCommand ?? (_playCommand = new RelayCommand(async obj =>
                {
                    await PlayAsync();
                    isTurningOn = true;
                }));
            }
        }
        private RelayCommand _pauseCommand;
        public RelayCommand PauseCommand
        {
            get
            {
                return _pauseCommand ?? (_pauseCommand = new RelayCommand(async obj =>
                {
                    await PauseAsync();
                }));
            }
        }
        private RelayCommand _openFileCommand;
        public RelayCommand OpenFileCommand
        {
            get
            {
                return _openFileCommand ?? (_openFileCommand = new RelayCommand(async obj =>
                {
                    await OpenFileAsync();
                }));
            }
        }
        //Методы
        private async Task OpenFileAsync() //Метод открытия команды ОТКРЫТЬ ФАЙЛ
        {
            const string settings = "Медиафайлы|*.mp3;*.flac;*.wav;*.mp4;*.mkv;*.avi|Все файлы|*.*";
            var result = _dialogService.OpenFiles(settings);

            if (result == null) return;

            foreach (var path in result)
            {
                try
                {
                    var mediaFile = await _fileService.ParseFileAsync(path);

                    PlayList.Add(mediaFile);

                    CurrentMedia = mediaFile; //Для движка 
                    _mediaEngine.Load(mediaFile.FilePath);
                    await PlayAsync();     
                }
                catch (Exception ex)
                {
                    _dialogService.ShowError($"Ошибка: {ex.Message}");
                }
            }
        }
        private async Task PlayAsync() //Метод RemoteControl: воспроизведения команды PlayCommand
        {

             await _mediaEngine.PlayAsync();
        }
        private async Task PauseAsync() //Метод RemoteControl: приостановление команды StopCommand 
        {
            await _mediaEngine.StopAsync();
        }
        //public isTurningOn ChangeUI()
        //{

        //}
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
