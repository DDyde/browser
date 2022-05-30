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
        ListBoxItem listBoxItem;
        private async void GetFavorites()
        {
            favoriteListPage.Items.Clear();
            DataTransfer dataTransfer = new DataTransfer();
            
            List<FavoriteDetails> favoriteDetails = await dataTransfer.GetFavoriteList();

            for (int i = 0; i < favoriteDetails.Count; i++)
            {
                listBoxItem = new ListBoxItem();
                listBoxItem.Style = Application.Current.Resources["favoriteListStyle"] as Style;
                listBoxItem.DataContext = favoriteDetails[i];
                listBoxItem.Tapped += ListBoxItem_Tapped;
                favoriteListPage.Items.Add(listBoxItem);
            }
        }

        private async void ListBoxItem_Tapped(object sender, TappedRoutedEventArgs e)
        {

            //var url = ((TextBlock)favoriteListPage.SelectedItem).link;

            ContentDialog chooseItem = new ContentDialog()
            {
                Title = "Выбор действия для поля",
                Content = "Необходимо выбрать действие для выбранного поля",
                PrimaryButtonText = "Перейти",
                SecondaryButtonText = "Удалить",
                CloseButtonText = "Отмена"
            };

            ContentDialogResult result = await chooseItem.ShowAsync();
            switch (result)
            {
                case ContentDialogResult.Primary:
                    break;
                case ContentDialogResult.Secondary:
                    DataTransfer dataTransfer = new DataTransfer();
                    //dataTransfer.DeleteSearchTerm();
                    break;
                default:
                    break;
            }
        }
    }
}
