using browser.history;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using muxc = Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace browser
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        int settingTabCount = 0;
        public string prefix = string.Empty;
        muxc.TabViewItem selectedTab = null;
        muxc.WebView2 selectedWebView = null;
        string homeUrl = "https://www.google.com";


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
            webBrowser.Reload();
        }

        private void searchBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Search();
            }
        }

        private async void Search()
        {
            DataTransfer dataTransfer = new DataTransfer();
            prefix = await dataTransfer.GetEngineAttribute("prefix");
            bool hasUrlType = await dataTransfer.HasUrlType(searchBox.Text);

            if (hasUrlType)
            {
                if (!searchBox.Text.Contains("http://") || !searchBox.Text.Contains("https://"))
                {
                    selectedWebView.CoreWebView2.Navigate(new Uri("https://www." + searchBox.Text).ToString());

                }
                else
                {
                    searchBox.Text = "https://www." + searchBox.Text;
                }
            }
            else
            {


                if (selectedWebView == null)
                {
                    webBrowser.Source = new Uri(prefix + searchBox.Text);
                }
                else
                {
                    selectedWebView.Source = new Uri(prefix + searchBox.Text);
                }
            }



        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            webBrowser.Source = new Uri(homeUrl);
        }

        private void settingMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (settingTabCount == 0)
            {
                AddSetingTab();
                settingTabCount++;
            }

        }

        private void AddSetingTab()
        {
            var settingsTab = new muxc.TabViewItem();
            settingsTab.Header = "Settings";
            settingsTab.Name = "settingsTab";
            settingsTab.IconSource = new muxc.SymbolIconSource() { Symbol = Symbol.Setting };
            Frame frame = new Frame();
            settingsTab.Content = frame;
            frame.Navigate(typeof(SettingPage));
            tabView.TabItems.Add(settingsTab);
            tabView.SelectedItem = settingsTab;
        }

        private void webBrowser_NavigationCompleted(muxc.WebView2 sender, CoreWebView2NavigationCompletedEventArgs args)
        {
            ToolTip toolTip = new ToolTip();

            try
            {
                searchBox.Text = webBrowser.Source.AbsoluteUri;
                DataTransfer dataTransfer = new DataTransfer();
                if (!string.IsNullOrEmpty(searchBox.Text))
                {
                    dataTransfer.saveSearchTerm(webBrowser.CoreWebView2.DocumentTitle, webBrowser.Source.AbsoluteUri);
                }

            }
            catch (Exception)
            {

                throw;
            }

            if (webBrowser.Source.AbsoluteUri.Contains("https"))
            {
                sslIcon.Source = new BitmapImage(new Uri("ms-appx:///Assets/icon/ico/lock.ico"));
                toolTip.Content = "This website has a SSL certefication";
                ToolTipService.SetToolTip(sslBth, toolTip);
            }
            else
            {
                sslIcon.Source = new BitmapImage(new Uri("ms-appx:///Assets/icon/ico/unlock.ico"));
                toolTip.Content = "This website hasn't a SSL certefication";
                ToolTipService.SetToolTip(sslBth, toolTip);
            }

            defaultTab.Header = webBrowser.CoreWebView2.DocumentTitle;

        }

        private void TabView_TabCloseRequested(muxc.TabView sender, muxc.TabViewTabCloseRequestedEventArgs args)
        {
            sender.TabItems.Remove(args.Tab);
            selectedTab = null;
            selectedWebView = null;

            if (args.Tab.Name == "settingsTab")
            {
                settingTabCount = 0;
            }
        }

        private void TabView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedTab = tabView.SelectedItem as muxc.TabViewItem;
            if (selectedTab != null)
            {
                selectedWebView = selectedTab.Content as muxc.WebView2;

            }

            if (selectedWebView != null)
            {
                searchBox.Text = selectedWebView.Source.AbsoluteUri;
            }
            else
            {
                searchBox.Text = " ";
            }


        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTransfer dataTransfer = new DataTransfer();
                string searchEngineName = await dataTransfer.GetEngineAttribute("name");
                prefix = await dataTransfer.GetEngineAttribute("prefix");

                searchBox.PlaceholderText = "search with " + searchEngineName + "...";
            }
            catch
            {

            }
        }


        private void favorite_Click(object sender, RoutedEventArgs e)
        {
            DataTransfer dataTransfer = new DataTransfer();
            dataTransfer.SaveFavorites(selectedWebView.Source.AbsoluteUri, selectedWebView.CoreWebView2.DocumentTitle);
        }

        private void TabView_AddTabButtonClick(muxc.TabView sender, object args)
        {
            var newTab = new muxc.TabViewItem();
            newTab.IconSource = new muxc.SymbolIconSource() { Symbol = Symbol.Add };

            muxc.WebView2 webView = new muxc.WebView2();
            newTab.Content = webView;
            webView.CoreWebView2.Navigate(new Uri("https://www.google.com").ToString());
            sender.TabItems.Add(newTab);
            sender.SelectedItem = newTab;
            webView.CoreWebView2.NavigationCompleted += BrowserNavigated;
        }

        private void BrowserNavigated(muxc.WebView2 sender, CoreWebView2NavigationCompletedEventArgs args)
        {
            var view = sender as muxc.WebView2;
            var tab = view.Parent as muxc.TabViewItem;
            tab.Header = view.CoreWebView2.DocumentTitle;
        }
    }
}
