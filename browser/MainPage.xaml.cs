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

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace browser
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        int settingTabCount = 0;
        int tabCount = 0;
        public string prefix = string.Empty;
        muxc.TabViewItem selectedTab = null;
        WebView selectedWebView = null;
        string homeUrl = string.Empty, homeName = string.Empty;


        public MainPage()
        {
            this.InitializeComponent();

            Data data = new Data();
            data.SettingsFiles();
            GetHome();
        }

        private async void GetHome()
        {
            try
            {
                DataTransfer dataTransfer = new DataTransfer();
                homeName = await dataTransfer.GetHomeAttribute("name");
                homeUrl = await dataTransfer.GetHomeAttribute("url");

            }
            catch (Exception ex)
            {
                MessageDialog messageDialog = new MessageDialog(ex.Message);
                await messageDialog.ShowAsync();
            }

            if (!string.IsNullOrEmpty(homeUrl) && !string.IsNullOrEmpty(homeName))
            {
                NavigateHome();
            }
        }

        private void NavigateHome()
        {
            if (selectedTab.Name != "settingsTab")
            {
                selectedWebView.Navigate(new Uri(homeUrl));
                selectedTab.Header = selectedWebView.DocumentTitle;
            }            
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTab.Name != "settingsTab")
            {
                if (selectedWebView.CanGoBack)
                {
                    selectedWebView.GoBack();
                }
            }
            
        }

        private void btnFrd_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTab.Name != "settingsTab")
            {
                if (selectedWebView.CanGoForward)
                {
                    selectedWebView.GoForward();
                }
            }
            
        }

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTab.Name != "settingsTab")
            {
                selectedWebView.Refresh();
            }
            
        }


        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            NavigateHome();
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
                    selectedWebView.Navigate(new Uri("https://www." + searchBox.Text));
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

        private void webBrowser_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {

            try
            {
                searchBox.Text = args.Uri.AbsoluteUri;
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

            CheckSSL();

            defaultTab.Header = webBrowser.DocumentTitle;

        }

        private void CheckSSL()
        {
            ToolTip toolTip = new ToolTip();
            if (selectedWebView.Source.AbsoluteUri.Contains("https"))
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
        }

        private void TabView_AddTabButtonClick(muxc.TabView sender, object args)
        {
            tabView.IsAddTabButtonVisible = false;
                AddNewTab(new Uri(homeUrl));           
        }

        private void BrowserNavigated(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            var view = sender as WebView;
            var tab = view.Parent as muxc.TabViewItem;
            tab.Header = view.DocumentTitle;
            searchBox.Text = view.Source.AbsoluteUri;
            CheckSSL();
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
                selectedWebView = selectedTab.Content as WebView;

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

        private void webBrowser_NewWindowRequested(WebView sender, WebViewNewWindowRequestedEventArgs args)
        {
            AddNewTab(args.Uri);
            args.Handled = true;
        }

        private void favorite_Click(object sender, RoutedEventArgs e)
        {
            DataTransfer dataTransfer = new DataTransfer();
            dataTransfer.SaveFavorites(selectedWebView.Source.AbsoluteUri, selectedWebView.DocumentTitle);

            notifyFav.Text = "Страница " + selectedWebView.DocumentTitle + " была добавлена в закладки. " + "\r\n" + selectedWebView.Source.AbsoluteUri;
        }

        private void AddNewTab(Uri Url)
        {   
            var newTab = new muxc.TabViewItem();
            newTab.IconSource = new muxc.SymbolIconSource() { Symbol = Symbol.Add };

            WebView webView = new WebView();
            newTab.Content = webView;
            webView.Navigate(Url);
            tabView.TabItems.Add(newTab);
            tabView.SelectedItem = newTab;
            webView.NavigationCompleted += BrowserNavigated;
            webView.NewWindowRequested += webBrowser_NewWindowRequested;
            tabView.IsAddTabButtonVisible = true;
        }
    }
}
