using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Data.Xml.Dom;

namespace browser.history
{
    internal class DataTransfer
    {
        string fileName = "settings.xml";

        public async void saveSearchTerm(string title, string url)
        {
            var document = await DocumentLoad().AsAsyncOperation();

            var history = document.GetElementsByTagName("history");

            XmlElement elementSiteName = document.CreateElement("sitename");
            XmlElement elementURL = document.CreateElement("url");

            var historyItem = history[0].AppendChild(document.CreateElement("historyitem"));

            historyItem.AppendChild(elementSiteName);
            historyItem.AppendChild(elementURL);

            elementSiteName.InnerText = title;
            elementURL.InnerText = url;

            SaveDocument(document);
        }

        private async Task<XmlDocument> DocumentLoad()
        {
            XmlDocument result = null;

            await Task.Run(async () =>
            {
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                XmlDocument document = await XmlDocument.LoadFromFileAsync(file);
                result = document;
            });

            return result;
        }

        private async void SaveDocument(XmlDocument document)
        {
            var file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
            await document.SaveToFileAsync(file);
        }

        public async Task<List<string>> Fetch(string source)
        {
            List<string> list = new List<string>();

            await Task.Run(async () =>
            {
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                XmlDocument document = await XmlDocument.LoadFromFileAsync(file);
                var historyItem = document.GetElementsByTagName("historyitem");

                for (int i = 0; i < historyItem.Count; i++)
                {
                    var historyChild = historyItem[i].ChildNodes;

                    for (int j = 0; j < historyChild.Count; j++)
                    {
                        if(historyChild[j].NodeName == source)
                        {
                            list.Add(historyChild[j].InnerText);
                        }
                    }
                }
            });

            return list;
        }
    }
}
