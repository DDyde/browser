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
                        writer.WriteStartElement("favorite");
                        writer.WriteEndElement();
                        writer.WriteStartElement("searchEngine");
                        writer.WriteStartElement("google");
                        writer.WriteAttributeString("prefix", "https://www.google.com/search?q=");
                        writer.WriteEndElement();
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
