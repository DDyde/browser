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
using browser.MenuPages;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace browser
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class SettingPage : Page
    {
        public SettingPage()
        {
            this.InitializeComponent();
        }

        private void navView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var selectedItem = (NavigationViewItem)args.SelectedItem;
            string tag = ((string)selectedItem.Tag);

            if (!args.IsSettingsSelected)
            {
                if (tag == "accountItem")
                {
                    contentFrame.Navigate(typeof(Account), null, args.RecommendedNavigationTransitionInfo);
                } else if (tag == "favoriteItem")
                {
                    contentFrame.Navigate(typeof(Favorite), null, args.RecommendedNavigationTransitionInfo);
                }
                else if (tag == "historyItem")
                {
                    contentFrame.Navigate(typeof(History), null, args.RecommendedNavigationTransitionInfo);
                }
                else if (tag == "settingItem")
                {
                    contentFrame.Navigate(typeof(Setting), null, args.RecommendedNavigationTransitionInfo);
                }
                
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void settingPage_Loaded(object sender, RoutedEventArgs e)
        {
            navView.SelectedItem = navView.MenuItems[0];
        }
    }
}
