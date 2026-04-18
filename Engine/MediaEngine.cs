using LibVLCSharp.Shared;
using NeonMediaApplication.Interfaces;
using NeonMediaApplication.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonMediaApplication.Engine
{
    //Класс: Движок медиаплеера VLC
    //Задача: воспроизведение
    //Библиотека LibVLCSharp - взаимодействие с движком для .NET (классы библиотеки: LibVLC, MediaPlayer)
    //Библиотека VideoLAN.LibVLC.Windows - двжиок на C++
    public class MediaEngine : IDisposable, IMediaEngine
    {
        //Поля
        private LibVLC _libVLC;
        private MediaPlayer _mediaPlayer;
        public LibVLCSharp.Shared.MediaPlayer MediaPlayer => _mediaPlayer; //Обьект плеера
        private Media _currentMedia; //поле плеера

        //Свойства
        public TimeSpan CurrentPosition //текущая позиция воспроизведения
        {
            get { return TimeSpan.FromMilliseconds(_mediaPlayer.Time); }
        }
        public TimeSpan Duration // общая длина воспроизведения
        {
            get
            {
                if (_currentMedia != null)
                    return TimeSpan.FromMilliseconds(_currentMedia.Duration);
                return TimeSpan.Zero;
            }
        }
        public bool IsPlaying //Флаг состояния проигрывания
        {
            get { return _mediaPlayer.IsPlaying; }
        }
        //События
        public event Action<TimeSpan> PositionChanged;
        public event Action MediaEnded;
        public event Action<bool> PlayingStateChanged;
        public event Action Paused;

        //Обработчики событий 
        private void OnMediaEnded(object sender, EventArgs e)
        {
            MediaEnded?.Invoke();
            PlayingStateChanged?.Invoke(false);
        }
        private void OnTimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            PositionChanged?.Invoke(TimeSpan.FromMilliseconds(e.Time));
        }

        public MediaEngine() //конструктор плеера
        {
            Core.Initialize(); // инициализация из LibVLCSharp

            _libVLC = new LibVLC(); // Создание движка VLC

            _mediaPlayer = new MediaPlayer(_libVLC); // Создание обьекта плеера
            //Подписка
            _mediaPlayer.EndReached += OnMediaEnded;
            _mediaPlayer.TimeChanged += OnTimeChanged;

            _mediaPlayer.Playing += (s, e) => PlayingStateChanged?.Invoke(true); //Подписка проигрывани
            _mediaPlayer.Stopped += (s, e) => PlayingStateChanged?.Invoke(false); //Подписка стоп
        }
        //Загрузка медиа фалйа в двжиок
        public async Task<bool> ReadMediaAsync() //Метод распаковки файла
        {
            if (_currentMedia == null)
            {
                return false;
            }

            try
            {
                if (_currentMedia.ParsedStatus != MediaParsedStatus.Done)
                {
                    await _currentMedia.Parse(MediaParseOptions.ParseNetwork);
                }
                return _currentMedia.ParsedStatus == MediaParsedStatus.Done;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public void Load(string filePath) // загрузка медиа файла в плеер
        {
            //_mediaPlayer.Stop();
            _currentMedia?.Dispose();
            _currentMedia = new Media(_libVLC, filePath); //создание нового обьекта для воспроизведения
        }
        //Метод работы воспроизведения плеера
        public async Task PlayAsync() // метод воспроизведения
        {
            if (_currentMedia == null) return;
            if (_mediaPlayer.State == VLCState.Paused)
                _mediaPlayer.Play();
            else
                _mediaPlayer.Play(_currentMedia);
        }
        public async Task PauseAsync() //метод паузы
        {
            _mediaPlayer.Pause();
        }
        public async Task StopAsync() //метод стоп
        {
            _mediaPlayer.Stop();
        }
        public async Task SetVolumeAsync(int volume) //Управление параметрами
        {
            _mediaPlayer.Volume = volume;
        }
        public async Task SeekAsync(TimeSpan position) //метод перемотки
        {
            _mediaPlayer.Time = (long)position.TotalMilliseconds;
        }
        public void Dispose() //Освобождение ресурсов 
        {
            _currentMedia?.Dispose();
            _mediaPlayer?.Dispose();
            _libVLC?.Dispose();
        }
    }
}
