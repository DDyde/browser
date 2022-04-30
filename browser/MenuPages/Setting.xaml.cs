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
    public sealed partial class Setting : Page
    {
        public Setting()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Setup();
            SetEngine();
        }

        private void SetEngine()
        {
            DataTransfer dataTransfer = new DataTransfer();
            searchComboBox.SelectedItem = dataTransfer.GetEngineAttribute("name");
        }

        private async void Setup()
        {
            DataTransfer dataTrandsfer = new DataTransfer();
            List<string> engineList = await dataTrandsfer.SearchEngineList("name");

            foreach (var item in engineList)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                comboBoxItem.Content = item;
                searchComboBox.Items.Add(comboBoxItem);
                if (await dataTrandsfer.GetEngineAttribute("name") == item)
                {
                    searchComboBox.SelectedItem = comboBoxItem;
                }
            }
        }

        private void searchComboBox_DropDownClosed(object sender, object e)
        {
            DataTransfer dataTransfer = new DataTransfer();
            var selectedItem = searchComboBox.SelectedValue as ComboBoxItem;
            dataTransfer.SetSearchEngine(selectedItem.Content.ToString());
        }
    }
}
