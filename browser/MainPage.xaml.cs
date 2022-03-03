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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using muxc = Microsoft.UI.Xaml.Controls;

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
            ToolTip toolTip = new ToolTip();

            if (webBrowser.Source.AbsoluteUri.Contains("https"))
            {
                sslIcon.Source = new BitmapImage(new Uri("ms-appx:///Assets/icon/ico/lock.ico"));
                toolTip.Content = "This website has a SSL certefication";
                ToolTipService.SetToolTip(sslBth, toolTip);
            } else
            {
                sslIcon.Source = new BitmapImage(new Uri("ms-appx:///Assets/icon/ico/unlock.ico"));
                toolTip.Content = "This website hasn't a SSL certefication";
                ToolTipService.SetToolTip(sslBth, toolTip);
            }

            try
            {
                searchBox.Text = webBrowser.Source.AbsoluteUri;

                DataTransfer dataTransfer = new DataTransfer();
                if (!string.IsNullOrEmpty(searchBox.Text))
                {
                    dataTransfer.saveSearchTerm(webBrowser.DocumentTitle, webBrowser.Source.AbsoluteUri);
                }
                
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private void TabView_AddTabButtonClick(muxc.TabView sender, object args)
        {
            var newTab = new muxc.TabViewItem();
            newTab.IconSource = new muxc.SymbolIconSource() { Symbol = Symbol.Add };

            WebView webView = new WebView();
            newTab.Content = webView;
            webView.Navigate(new Uri("https://www.google.com"));
            sender.TabItems.Add(newTab);
            sender.SelectedItem = newTab;
        }

        private void TabView_TabCloseRequested(muxc.TabView sender, muxc.TabViewTabCloseRequestedEventArgs args)
        {
            sender.TabItems.Remove(args.Tab);
        }
    }
}
