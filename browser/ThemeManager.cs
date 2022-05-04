using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace browser
{
    public sealed class ThemeManager : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
                    => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public const string MainThemePath = "ms-appx:///Themes/Theme.Main.xaml";
        public const string SunsetThemePath = "ms-appx:///Themes/Theme.Sunset.xaml";
        public const string PurpleThemePath = "ms-appx:///Themes/Theme.Purple.xaml";
        
        private ResourceDictionary _currentThemeDictionary;

        public string CurrentTheme { get; private set; }

        public Brush wbBackGround => _currentThemeDictionary[nameof(wbBackGround)] as Brush;
        public Brush wbRowLine => _currentThemeDictionary[nameof(wbRowLine)] as Brush;
        public Brush wbGreen => _currentThemeDictionary[nameof(wbGreen)] as Brush;
        public Brush wbForeground => _currentThemeDictionary[nameof(wbForeground)] as Brush;

        private void RaisePropertyChanged()
        {
            OnPropertyChanged(nameof(wbBackGround));
            OnPropertyChanged(nameof(wbRowLine));
            OnPropertyChanged(nameof(wbGreen));
            OnPropertyChanged(nameof(wbForeground));
            OnPropertyChanged(nameof(CurrentTheme));
        }

        public void LoadTheme(string path)
        {
            _currentThemeDictionary = new ResourceDictionary();
            App.LoadComponent(_currentThemeDictionary, new Uri(path));
            CurrentTheme = Path.GetFileNameWithoutExtension(path);

            RaisePropertyChanged();
        }

        public ThemeManager()
        {
            LoadTheme(SunsetThemePath);
        }
    }
}
