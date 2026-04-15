using LibVLCSharp.Shared;
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
            _mediaEngine.PositionChanged += pos =>
            {
                Application.Current.Dispatcher.Invoke(() => CurrentPosition = pos);
            };

            _mediaEngine.MediaEnded += OnMediaEnded;
        }
        private double _seekPosition;
        public double SeekPosition
        {
            get => _seekPosition;
            set
            {
                if (Math.Abs(_seekPosition - value) < 0.01) return;
                _seekPosition = value;
                OnPropertyChanged();
                _mediaEngine.SeekAsync(TimeSpan.FromSeconds(value));
            }
        }
        private TimeSpan _duration;
        public TimeSpan Duration
        {
            get => _duration;
            set
            {
                _duration = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DurationSeconds));
            }
        }
        public double DurationSeconds => Duration.TotalSeconds;
        private TimeSpan _currentPosition;
        public TimeSpan CurrentPosition
        {
            get => _currentPosition;
            private set
            {
                _currentPosition = value;
                OnPropertyChanged();
                _seekPosition = value.TotalSeconds;
                OnPropertyChanged(nameof(SeekPosition));
            }
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
        private RelayCommand _playCommand; //Плэй
        public RelayCommand PlayCommand //Плэй
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
        private RelayCommand _pauseCommand; //Пауза
        public RelayCommand PauseCommand //Пауза
        {
            get
            {
                return _pauseCommand ?? (_pauseCommand = new RelayCommand(async obj =>
                {
                    await PauseAsync();
                }));
            }
        }
        private RelayCommand _openFileCommand; //Открыть файл
        public RelayCommand OpenFileCommand //Открыть файл
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

        //private async Task OpenFileAsync() //Метод открытия команды ОТКРЫТЬ ФАЙЛ
        //{
        //    const string settings = "Медиафайлы|*.mp3;*.flac;*.wav;*.mp4;*.mkv;*.avi|Все файлы|*.*";
        //    var result = _dialogService.OpenFiles(settings);

        //    if (result == null) return;

        //    foreach (var path in result)
        //    {
        //        try
        //        {
        //            var mediaFile = await _fileService.ParseFileAsync(path);

        //            PlayList.Add(mediaFile);

        //            CurrentMedia = mediaFile; //Для движка 
        //            _mediaEngine.Load(mediaFile.FilePath);
        //            await PlayAsync();     
        //        }
        //        catch (Exception ex)
        //        {
        //            _dialogService.ShowError($"Ошибка: {ex.Message}");
        //        }
        //    }
        //}
        private async Task OpenFileAsync() //Метод открытия команды ОТКРЫТЬ ФАЙЛ
        {
            const string settings = "Медиафайлы|*.mp3;*.flac;*.wav;*.mp4;*.mkv;*.avi|Все файлы|*.*";
            var result = _dialogService.OpenFiles(settings);

            if (result == null || result.Length == 0) return;

            foreach (var path in result)
            {
                try
                {
                    var mediaFile = await _fileService.ParseFileAsync(path);
                    PlayList.Add(mediaFile);
                }
                catch (Exception ex)
                {
                    _dialogService.ShowError($"Ошибка при добавлении файла {path}: {ex.Message}");
                }
            }

            if (PlayList.Count > 0)
            {
                CurrentMedia = PlayList.Last();
                _mediaEngine.Load(CurrentMedia.FilePath);
                await _mediaEngine.ReadMediaAsync();
                Duration = _mediaEngine.Duration;
                await _mediaEngine.PlayAsync();
                isTurningOn = true;
            }
        }
        private async Task PlayAsync() //Метод RemoteControl: воспроизведения команды PlayCommand
        {
            if(isTurningOn == false)
            {
                await _mediaEngine.PlayAsync();
                isTurningOn = true;
            }
        }
        private async Task PauseAsync() //Метод RemoteControl: приостановление команды StopCommand 
        {
            if(isTurningOn == true)
            {
                await _mediaEngine.PauseAsync();
                isTurningOn = false;
            }
        }
        private void OnMediaEnded()
        {
            Application.Current.Dispatcher.Invoke(async () =>
            {
                if (PlayList.Count == 0 || CurrentMedia == null) return;

                int currentIndex = PlayList.IndexOf(CurrentMedia);
                int nextIndex = currentIndex + 1;

                if (nextIndex < PlayList.Count)
                {
                    var nextMedia = PlayList[nextIndex];
                    CurrentMedia = nextMedia;
                    _mediaEngine.Load(nextMedia.FilePath);
                    await _mediaEngine.ReadMediaAsync();
                    Duration = _mediaEngine.Duration;

                    await _mediaEngine.PlayAsync();
                    isTurningOn = true;
                }
                else
                {
                    await _mediaEngine.StopAsync();
                    isTurningOn = false;
                    CurrentPosition = TimeSpan.Zero;
                }
            });
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
