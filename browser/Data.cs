using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using Windows.Storage;
using Windows.Storage.Streams;

namespace browser
{
    public class Data
    {
        public async void SettingsFiles()
        {
            try
            {
                var storageFile = await ApplicationData.Current.LocalFolder.CreateFileAsync("settings.xml");

                using (IRandomAccessStream writeStream = await storageFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    Stream srm = writeStream.AsStreamForWrite();
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Async = true;
                    settings.Indent = true;

                    using(XmlWriter writer = XmlWriter.Create(srm, settings))
                    {
                        writer.WriteStartDocument();
                        writer.WriteStartElement("settings");
                        writer.WriteStartElement("history");
                        writer.WriteEndElement();
                        writer.WriteStartElement("favorites");
                        writer.WriteEndElement();
                        writer.WriteStartElement("searchEngine");
                        writer.WriteStartElement("engine");
                        writer.WriteAttributeString("prefix", "https://www.google.com/search?q=");
                        writer.WriteAttributeString("name", "Google");
                        writer.WriteAttributeString("selected", "True");
                        writer.WriteEndElement();
                        writer.WriteStartElement("engine");
                        writer.WriteAttributeString("prefix", "https://duckduckgo.com/?q=");
                        writer.WriteAttributeString("name", "DuckduckGo");
                        writer.WriteAttributeString("selected", "False");
                        writer.WriteEndElement();
                        writer.WriteStartElement("engine");
                        writer.WriteAttributeString("prefix", "https://yandex.ru/search/?text=");
                        writer.WriteAttributeString("name", "Yandex");
                        writer.WriteAttributeString("selected", "False");
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                        writer.WriteStartElement("types");
                        writer.WriteStartElement("type");
                        writer.WriteAttributeString("name", ".com");
                        writer.WriteEndElement();
                        writer.WriteStartElement("type");
                        writer.WriteAttributeString("name", ".ru");
                        writer.WriteEndElement();
                        writer.WriteStartElement("type");
                        writer.WriteAttributeString("name", ".net");
                        writer.WriteEndElement();
                        writer.WriteStartElement("type");
                        writer.WriteAttributeString("name", ".us");
                        writer.WriteEndElement();
                        writer.WriteStartElement("type");
                        writer.WriteAttributeString("name", ".org");
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                        writer.WriteStartElement("home");
                        writer.WriteAttributeString("name", "home");
                        writer.WriteAttributeString("url", "https://www.google.com");
                        writer.WriteEndElement();
                        writer.WriteStartElement("newtab");
                        writer.WriteAttributeString("title", "");
                        writer.WriteAttributeString("url", "");
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                        writer.Flush();
                        await writer.FlushAsync();
                    }
                }

                await Windows.System.Launcher.LaunchFileAsync(storageFile);

            }
            catch (Exception ex)
            {

            }
        }
    }
}
