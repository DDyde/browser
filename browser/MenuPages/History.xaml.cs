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
    public sealed partial class History : Page
    {

        int listBoxItemCount  = 0;

        public History()
        {
            this.InitializeComponent();
        }
        
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataTransfer dataTransfer = new DataTransfer();
            List<string> historyUrlItem = await dataTransfer.Fetch("url");
            foreach (var item in historyUrlItem)
            {
                ListBoxItem listBoxItem = new ListBoxItem();
                listBoxItem.Name = "newListBoxItem" + listBoxItemCount;
                listBoxItemCount++;
                listBoxItem.Tapped += listBoxItem_Tapped;
                Style style = Application.Current.Resources["historyList"] as Style;
                listBoxItem.Style = style;
                listBoxItem.Content = item;
                listHistory.Items.Add(listBoxItem);
            }
        }

        private async void listBoxItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ContentDialog chooseItem = new ContentDialog() {
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
                    dataTransfer.DeleteSearchTerm("https://yandex.ru/search/?text=toontune+animation&lr=213");
                    break;
                default:
                    break;
            }

        }
    }
}
