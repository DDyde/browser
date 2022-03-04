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
                Style style = Application.Current.Resources["historyList"] as Style;
                listBoxItem.Style = style;
                listBoxItem.Content = item;
                listHistory.Items.Add(listBoxItem);
            }
        }
    }
}
