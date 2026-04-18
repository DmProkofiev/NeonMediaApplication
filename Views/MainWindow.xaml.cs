using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NeonMediaApplication.ViewModels;
using NeonMediaApplication.Engine;
using NeonMediaApplication.Interfaces;

namespace NeonMediaApplication.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IMediaEngine _mediaEngine; //Поле обьекта(ссылка на движок)
        public LibVLCSharp.Shared.MediaPlayer MediaPlayer => _mediaEngine.MediaPlayer; // Свойство возврат объект MediaPlayer из движка
        public MainWindow(IMediaEngine mediaEngine) 
        {
            _mediaEngine = mediaEngine;
            InitializeComponent();
            VideoView.MediaPlayer = mediaEngine.MediaPlayer;
        }
    }
}