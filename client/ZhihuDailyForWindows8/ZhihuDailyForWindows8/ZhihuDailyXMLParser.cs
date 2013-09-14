/**
 * Author : ihainan
 * E-mail : ihainan72@gmail.com
 * Created Time : 2013/09/08 17:21
 * Website : http://www.ihainan.me
 **/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Storage;

namespace ZhihuDailyForWindows8
{
    class ZhihuDailyXMLParser
    {
        String titlesAPIUrl = "http://zhdaily.sinaapp.com/getTitles.php";		// The url of Our API which we get the news from
        String contentAPIUrl = "http://zhdaily.sinaapp.com/getNewsContent.php";
        ZhihuDailyFileManager fm = new ZhihuDailyFileManager();                 // File Manager
        /* 
         * get the title of a special day's news 
        */
        public async Task<List<Dictionary<string, string>>> getTitles(DateTime dateTime, bool isOnlineDownload = false)
        {
            /* covert date to string */
            DateTime date = dateTime.Date;
            String dateString = dateTime.ToString("yyyyMMdd");
            if (dateString != null)
            {
                titlesAPIUrl = titlesAPIUrl + "?date=" + dateString;
            }
            string fileName = dateString + ".xml";

            /* load XML */
            string now = DateTime.Now.Date.ToString("yyyyMMdd");
            List<Dictionary<string, string>> aRoot = new List<Dictionary<string, string>>();
            Dictionary<String, String> dElement;
            XmlDocument xmlDoc;
            if (dateString == now || dateString != now && !(await fm.isExisted(fileName)))
            {
                var connectionProfile = Windows.Networking.Connectivity.NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null)
                {
                    var messageDialog = new Windows.UI.Popups.MessageDialog("网络连接错误，请检查您的网络再重新尝试该操作");
                    var result = messageDialog.ShowAsync();
                    return null;
                }
                // download from the internet
                Uri uri = new Uri(titlesAPIUrl);
                try
                {
                    xmlDoc = await XmlDocument.LoadFromUriAsync(uri);
                    if (xmlDoc != null)
                    {
                        Windows.Storage.StorageFolder file = ApplicationData.Current.LocalFolder;
                        StorageFile st = await file.CreateFileAsync(fileName, Windows.Storage.CreationCollisionOption.ReplaceExisting);
                        await xmlDoc.SaveToFileAsync(st);
                    }
                    else
                        return null;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            else
            {
                // load from local file
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                xmlDoc = await XmlDocument.LoadFromFileAsync(file);
            }

            /* covert XML to List */
            XmlNodeList xmlNodeList = xmlDoc.GetElementsByTagName("titles");
            foreach(IXmlNode xmlNode in xmlNodeList){
				foreach(IXmlNode  xmlChild in xmlNode.ChildNodes){
                    dElement = new Dictionary<string, string>();
                    foreach (IXmlNode xmlGrandson in xmlChild.ChildNodes)
                    {
                        dElement.Add(xmlGrandson.NodeName, xmlGrandson.InnerText);
                    }
                    aRoot.Add(dElement);
				}
			}
            return aRoot;
        }

        /* get the contents of special news */
        public async Task<Dictionary<string, string>> getNewsContent(String url)
        {
            string fileName = System.Net.WebUtility.UrlEncode(url) + ".xml";
            XmlDocument xmlDoc;
            // load xml file
            if (await fm.isExisted(fileName))
            {
                // load from local file
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                xmlDoc = await XmlDocument.LoadFromFileAsync(file);
            }
            else
            {
                var connectionProfile = Windows.Networking.Connectivity.NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null)
                {
                    var messageDialog = new Windows.UI.Popups.MessageDialog("网络连接错误，请检查您的网络再重新尝试该操作");
                    var result = messageDialog.ShowAsync();
                    return null;
                }
                // download from the internet
                Uri uri = new Uri(contentAPIUrl + "?url=" + url);
                try
                {
                    xmlDoc = await XmlDocument.LoadFromUriAsync(uri);
                    if (xmlDoc != null)
                    {
                        Windows.Storage.StorageFolder file = ApplicationData.Current.LocalFolder;
                        StorageFile st = await file.CreateFileAsync(fileName, Windows.Storage.CreationCollisionOption.ReplaceExisting);
                        await xmlDoc.SaveToFileAsync(st);
                    }
                    else
                        return null;
                }
                catch(Exception e)
                {
                    return null;
                }
            }

            /* Covert XML to Array */
            Dictionary<string, string> dElement = new Dictionary<string, string>();
            XmlNodeList xmlNodeList = xmlDoc.GetElementsByTagName("news");
            foreach (IXmlNode xmlNode in xmlNodeList)
            {
                foreach (IXmlNode xmlChild in xmlNode.ChildNodes)
                {
                    dElement.Add(xmlChild.NodeName, xmlChild.InnerText);
                }
            }
            return dElement;
        }
    }
}
