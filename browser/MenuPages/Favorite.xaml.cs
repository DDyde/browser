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
using browser.history;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace browser.MenuPages
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class Favorite : Page
    {
        public Favorite()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetFavorites();
        }

        private async void GetFavorites()
        {
            favoriteListPage.Items.Clear();
            DataTransfer dataTransfer = new DataTransfer();
            
            List<FavoriteDetails> favoriteDetails = await dataTransfer.GetFavoriteList();

            for (int i = 0; i < favoriteDetails.Count; i++)
            {
                ListBoxItem listBoxItem = new ListBoxItem();
                listBoxItem.Style = Application.Current.Resources["favoriteListStyle"] as Style;
                listBoxItem.DataContext = favoriteDetails[i];
                favoriteListPage.Items.Add(listBoxItem);
            }
        }
    }
}
