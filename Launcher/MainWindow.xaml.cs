using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Launcher.ViewModels;

namespace Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _medPlay.Source = new Uri(Directory.GetCurrentDirectory() + "/vid.mp4", UriKind.Absolute);
            _medPlay.Play();
            var b = _medPlay.IsLoaded;
            _medPlay.MediaEnded += _medPlay_MediaEnded;

            DataContext = new MainWindowViewModel();
        }

        private void _medPlay_MediaEnded(object sender, RoutedEventArgs e)
        {
            try
            {
                _medPlay.Position = new TimeSpan(0, 0, 1);
                _medPlay.Play();
            }
            catch (Exception)
            {
            }
        }
    }
}
