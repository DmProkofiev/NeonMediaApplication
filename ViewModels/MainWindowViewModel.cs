using NeonMediaApplication.Interfaces;
using NeonMediaApplication.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace NeonMediaApplication.ViewModels
{
    //Напоминание: сделать коллекцию для обложки CoverArt BitMap из массивов байтов !!!
    //Напоминание: рассмотреть паттерн наблюдателя
    //Напоминание: разобраться с слайдером перемотки: перемотка происходит на установленную широкую частоту, при частой перемотке - происходит зависание
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        //Поля
        private readonly IMediaEngine _mediaEngine; //обьект движка VLC
        private readonly IFileService _fileService; // интерфейс файлового сервиса
        private readonly IDialogService _dialogService; // интерфейс диалога
        private readonly IExportAudioService _exportAudioService; // кспорт аудио из видео

        //Коллекции
        private ObservableCollection<MediaFile> PlayList { get; set; } = new ObservableCollection<MediaFile>(); //Коллекция для хранения плейлиста общая
        public ObservableCollection<string> PopupMenuItems { get; } = new ObservableCollection<string>();
        public MainWindowViewModel(IMediaEngine mediaEngine, IFileService fileService, IDialogService dialogService, IExportAudioService exportAudioService) //Конструктор DI
        {
            _mediaEngine = mediaEngine;
            _fileService = fileService;
            _dialogService = dialogService;
            _exportAudioService = exportAudioService;

            _mediaEngine.SetVolumeAsync(_volume); //Звук
            _mediaEngine.PositionChanged += pos => { Application.Current.Dispatcher.Invoke(() => CurrentPosition = pos); }; //Подписка на уведомление текущей позиции

            _mediaEngine.MediaEnded += OnMediaEnded;

            _mediaEngine.PlayingStateChanged += state => { Application.Current.Dispatcher.Invoke(() => IsPlaying = state); }; //Подписка на уведомление состояния проигрывания

        }
        // Observable properties backing fields
        private bool _isPopUp; // Флаг состояния выпадающего окна
        public bool IsPopUp
        {
            get => _isPopUp;
            set
            {
                _isPopUp = value;
                OnPropertyChanged();
            }
        }
        private bool _isPlaying = false; //Флаг состояния проигрывания
        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                _isPlaying = value;
                OnPropertyChanged();
            }
        }
        private double _seekPosition;//Двустороняя привязка 
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
        private TimeSpan _duration; //Длительность медиафайла
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
        private TimeSpan _currentPosition; // текущая позиция воспроизведения
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
        private int _volume = 70; // Громкость
        public int Volume
        {
            get => _volume;
            set
            {
                if (_volume == value) return;
                _volume = value;
                OnPropertyChanged();
                _mediaEngine.SetVolumeAsync(value);
            }
        }
        private string _file; //Файл парсинга
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
        private RelayCommand _popUpCommand; // Команда для вызова диалога дополнительного функционала
        public RelayCommand PopUpCommand
        {
            get
            {
                return _popUpCommand ?? (_popUpCommand = new RelayCommand(async obj =>
                {
                    IsPopUp = !IsPopUp;
                    UpdatePopupMenu();
                }));
            }
        }
        public ICommand ExecutePopupItemCommand => new RelayCommand(param => // Логика  выбора пункта меню
        {
            if (param is string menuItem)
            {
                IsPopUp = false;
                switch (menuItem)
                {
                    case "Экспорт аудио":
                        ExportAsync();
                        break;
                    case "Редактировать метаданные":
                        //EditMetadataLogic();
                        break;
                }
            }
        });
        private RelayCommand _playCommand; //Плэй
        public RelayCommand PlayCommand //Плэй
        {
            get
            {
                return _playCommand ?? (_playCommand = new RelayCommand(async obj =>
                {
                    await PlayAsync();
                    _isPlaying = true;
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
        private RelayCommand _stopCommand;
        public RelayCommand StopCommand
        {
            get
            {
                return _stopCommand ?? (_stopCommand = new RelayCommand(async obj =>
                {
                    await StopAsync();
                }));
            }
        }
        //Методы Команд
        private void UpdatePopupMenu()
        {
            PopupMenuItems.Clear();

            if (CurrentMedia is Video)
            {
                PopupMenuItems.Add("Экспорт аудио");
                PopupMenuItems.Add("Редактировать метаданные");
            }
            else if (CurrentMedia is Audio)
            {
                PopupMenuItems.Add("Редактировать метаданные");
            }
        }
        private async Task OpenFileAsync() //Метод открытия команды ОТКРЫТЬ ФАЙЛ
        {
            //Логика действий:: вызвать диалоговое окно, принять файл, проивзести парсинг, добавить в плейлист, проивзести загрузку в медиаплеер, начать чтение!!
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
            }
        }
        private async Task PlayAsync() //Метод RemoteControl: воспроизведения команды PlayCommand
        {

            if (_isPlaying == false)
            {
                await _mediaEngine.PlayAsync();
                _isPlaying = true;
            }
        }
        private async Task PauseAsync() //Метод RemoteControl: приостановление команды PauseCommand 
        {

            if (_isPlaying == true)
            {
                await _mediaEngine.PauseAsync();
                _isPlaying = false;
            }
        }
        private async Task StopAsync() //Метод RemoteControl: остановление команды StopCommand
        {
            if (_isPlaying == true)
            {
                await _mediaEngine.StopAsync();
            }

        }
        private async Task ExportAsync() // Метод для экспорта
        {
            if (!(CurrentMedia is Video video))
            {
                _dialogService.ShowError("Экспорт доступен только для видеофайлов.");
                return;
            }

            string filter = "MP3 файлы|*.mp3|AAC файлы|*.aac|FLAC файлы|*.flac|OGG файлы|*.ogg";
            string outputPath = _dialogService.SaveFile(filter, "mp3", "Сохранить аудио");
            if (string.IsNullOrEmpty(outputPath))
                return;

            try
            {

                await _exportAudioService.ExportAudioAsync(video.FilePath, outputPath);
                _dialogService.ShowMessage("Экспорт аудио успешно завершён.");
            }
            catch (Exception ex)
            {
                _dialogService.ShowError($"Ошибка экспорта: {ex.Message}");
            }

        }
        //Обработчики События
        private void OnMediaEnded() //Метод для обработки события окончания воспроизведения
        {
            Application.Current.Dispatcher.InvokeAsync(async () =>
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
                }
                else
                {
                    await _mediaEngine.StopAsync();
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
