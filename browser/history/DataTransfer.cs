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
                var document = await DocumentLoad();
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
                var document = await DocumentLoad();

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

        public async void DeleteSearchTerm(string Url)
        {
            var document = await DocumentLoad();
            var history = document.GetElementsByTagName("history");
            var historyItem = history[0].ChildNodes;
            for (int i = 0; i < historyItem.Count; i++)
            {
                if (historyItem[i].NodeName == "historyitem")
                {
                    var historyUrl = historyItem[i].ChildNodes;
                    for (int j = 0; j < historyUrl.Count; j++)
                    {
                        if (historyUrl[j].NodeName == "url" && historyUrl[j].InnerText == Url)
                        {
                            historyUrl[j].ParentNode.ParentNode.RemoveChild(historyItem[i]);
                        }
                    }
                    
                }
            }
            SaveDocument(document);
        }

        public async void RemoveFavorite(string Url)
        {
            var document = await DocumentLoad();
            var favorite = document.GetElementsByTagName("favorite");

            for (int i = 0; i < favorite.Count; i++)
            {
                var favoriteChild = favorite[i].ChildNodes;

                for (int j = 0; j < favoriteChild.Count; j++)
                {
                    if (favoriteChild[j].NodeName == "url" && favoriteChild[j].InnerText == Url)
                    {
                        favoriteChild[j].ParentNode.ParentNode.RemoveChild(favorite[i]);
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



        public async Task<bool> HasUrlType(string searchString)
        {
            bool result = false;

            await Task.Run(async () =>
            {
                var document = await DocumentLoad();
                var types = document.GetElementsByTagName("types");
                var typeChildren = types[0].ChildNodes;

                for (int i = 0; i < typeChildren.Count; i++)
                {
                    if (typeChildren[i].NodeName == "type")
                    {
                        if (searchString.Contains(typeChildren[i].Attributes.GetNamedItem("name").InnerText))
                        {
                            result = true;
                        }
                    }
                }
            });

            return result;
        }

        public async Task<string> GetHomeAttribute(string Source)
        {
            string result = "";

            await Task.Run(async () =>
            {
                var document = await DocumentLoad();
                var home = document.GetElementsByTagName("home");
                result = home[0].Attributes.GetNamedItem(Source).InnerText;
            });

            return result;
        }

        public async void  SaveFavorites(string Url, string Title)
        {
            var document = await DocumentLoad();

            var favorites = document.GetElementsByTagName("favorites");
            var favorite = favorites[0].AppendChild(document.CreateElement("favorite"));
            var favoriteUrl = favorite.AppendChild(document.CreateElement("url"));
            var favoriteTitle = favorite.AppendChild(document.CreateElement("title"));

            favoriteTitle.InnerText = Title;
            favoriteUrl.InnerText = Url;

            SaveDocument(document);
        }

        public async Task<List<FavoriteDetails>> GetFavoriteList()
        {
            List<FavoriteDetails> favoriteList = new List<FavoriteDetails>();

            await Task.Run(async () =>
            {
                var document = await DocumentLoad();

                var favorite = document.GetElementsByTagName("favorite");
                for (int i = 0; i < favorite.Count; i++)
                {
                    var favoriteChild = favorite[i].ChildNodes;

                    string returnUrl = string.Empty;
                    string returnTitle = string.Empty;

                    if (favorite[i].NodeName == "favorite")
                    {
                        for (int j = 0; j < favoriteChild.Count; j++)
                        {
                            if (favoriteChild[j].NodeName == "url")
                            {
                                returnUrl = favoriteChild[j].InnerText;
                            }

                            if (favoriteChild[j].NodeName == "title")
                            {
                                returnTitle = favoriteChild[j].InnerText;
                            }
                        }
                    }

                    if (returnUrl != string.Empty && returnTitle != string.Empty)
                    {
                        favoriteList.Add(new FavoriteDetails { Title = returnTitle, Url = returnUrl });
                    }
                }
            });

            return favoriteList;
        }
    }

    public class FavoriteDetails
    {
        public string Url { get; set; }
        public string Title { get; set; }
    }
}
