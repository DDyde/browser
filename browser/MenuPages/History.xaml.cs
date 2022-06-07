using browser.history;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace browser.MenuPages
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class History : Page
    {

        int listBoxItemCount = 0;
        MainPage mainPage;

        public History()
        {
            this.InitializeComponent();
        }

        ListBoxItem listBoxItem;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadListBox();
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
                    int index = listHistory.SelectedIndex;
                    listBoxItem = (ListBoxItem)
                        (listHistory.ItemContainerGenerator.ContainerFromIndex(index));
                    string url = (listBoxItem.Content.ToString());
                    DataTransfer dataTransfer = new DataTransfer();
                    dataTransfer.DeleteSearchTerm(url);
                    break;
                default:
                    break;
            }
            LoadListBox();

        }

        private async void LoadListBox()
        {
            listHistory.Items.Clear();
            DataTransfer dataTransfer = new DataTransfer();
            List<string> historyUrlItem = await dataTransfer.Fetch("url");
            foreach (var item in historyUrlItem)
            {
                listBoxItem = new ListBoxItem();
                listBoxItem.Name = "newListBoxItem" + listBoxItemCount;
                listBoxItemCount++;
                listBoxItem.Tapped += listBoxItem_Tapped;
                Style style = Application.Current.Resources["historyList"] as Style;
                listBoxItem.Style = style;
                listBoxItem.Content = item;
                listHistory.Items.Add(listBoxItem);
            }
        }
    }
}
