using browser.history;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace browser
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            Data data = new Data();
            data.SettingsFiles();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (webBrowser.CanGoBack)
            {
                webBrowser.GoBack();
            }            
        }

        private void btnFrd_Click(object sender, RoutedEventArgs e)
        {
            if (webBrowser.CanGoForward)
            {
                webBrowser.GoForward();
            }
        }

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            webBrowser.Refresh();
        }

        private void searchBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Search();
            }
        }

        private void Search()
        {
            webBrowser.Source = new Uri("https://www.google.com/search?q=" + searchBox.Text);
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            webBrowser.Source = new Uri("https://www.google.com/");
        }

        private void settingMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingPage));
        }

        private void webBrowser_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            try
            {
                searchBox.Text = webBrowser.Source.AbsoluteUri;

                DataTransfer dataTransfer = new DataTransfer();
                if (!string.IsNullOrEmpty(searchBox.Text))
                {
                    dataTransfer.saveSearchTerm(searchBox.Text, webBrowser.DocumentTitle, webBrowser.Source.AbsoluteUri);
                }
                
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}
