using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Data.Xml.Dom;
using Windows.System;

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
                        if (historyChild[j].NodeName == source)
                        {
                            list.Add(historyChild[j].InnerText);
                        }
                    }
                }
            });

            return list;
        }

        public async void LoadXmlFile()
        {
            var file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
            await Launcher.LaunchFileAsync(file);
        }

        public async Task<List<string>> SearchEngineList(string AttributeSource)
        {
            List<string> list = new List<string>();

            await Task.Run(async () =>
            {
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                XmlDocument document = await XmlDocument.LoadFromFileAsync(file);

                var searchEngine = document.GetElementsByTagName("searchEngine");

                var searchChild = searchEngine[0].ChildNodes;

                for (int j = 0; j < searchChild.Count; j++)
                {
                    if (searchChild[j].NodeName == "engine")
                    {
                        list.Add(searchChild[j].Attributes.GetNamedItem(AttributeSource).InnerText);
                    }
                }
            });

            return list;
        }

        public async void SetSearchEngine(string engineName)
        {
            var document = await DocumentLoad();
            var searchEngine = document.GetElementsByTagName("searchEngine");
            var engines = searchEngine[0].ChildNodes;
            for (int i = 0; i < engines.Count; i++)
            {

                if (engines[i].NodeName == "engine")
                {
                    if (engines[i].Attributes.GetNamedItem("name").InnerText == engineName)
                    {
                        engines[i].Attributes.GetNamedItem("selected").InnerText = true.ToString();
                    }
                    else
                    {
                        engines[i].Attributes.GetNamedItem("selected").InnerText = false.ToString();
                    }
                }

            }

            SaveDocument(document);
        }

        public async Task<string> GetEngineAttribute(string AttributeName)
        {
            string value = string.Empty;

            await Task.Run(async () =>
            {
                var document = await DocumentLoad();
                var searchEngine = document.GetElementsByTagName("searchEngine");
                var engines = searchEngine[0].ChildNodes;

                for (int i = 0; i < engines.Count; i++)
                {
                    if (engines[i].NodeName == "engine")
                    {
                        if (engines[i].Attributes.GetNamedItem("selected").InnerText == true.ToString())
                        {
                            value = engines[i].Attributes.GetNamedItem(AttributeName).InnerText;
                        }
                    }
                }
            });

            return value;
        }
    }
}
