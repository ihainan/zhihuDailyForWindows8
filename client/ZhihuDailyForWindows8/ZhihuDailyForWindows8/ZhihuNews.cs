/**
 * Author : ihainan
 * E-mail : ihainan72@gmail.com
 * Created Time : 2013/09/08 17:21
 * Website : http://www.ihainan.me
 **/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace ZhihuDailyForWindows8
{
    /* Holds all the news */
    class ZhihuNews
    {
        static public int defaultDaysOfNews = 0;                   // get the last 5 days' news acquiescently
        public List<OneDayNews> latestNews = new List<OneDayNews>();

        /* load news and add them into latestNews list */
        public async Task<bool> loadNews(DateTime day1, bool isOnlineDownload = false)
        {
            DateTime day2 = day1.AddDays(-defaultDaysOfNews);
            for (DateTime day = day1; day2.CompareTo(day) <= 0; day = day.AddDays(-1))
            {
                OneDayNews oneDayNews = new OneDayNews(day);
                if (await oneDayNews.updateNews(isOnlineDownload) == false)
                    return false;
                latestNews.Add(oneDayNews);
            }
            return true;
        }

        /* update all the news */
        public async Task<bool> updateNews(DateTime day1)
        {
            DateTime day2 = day1.AddDays(-defaultDaysOfNews);
            latestNews = new List<OneDayNews>();
            for (DateTime day = day1; day2.CompareTo(day) <= 0; day = day.AddDays(-1))
            {
                OneDayNews oneDayNews = new OneDayNews(day);
                if (await oneDayNews.updateNews() == false)
                    return false;
                latestNews.Add(oneDayNews);
            }
            return true;
        }

        /* load the extra data of all the news */
        public async Task<bool> loadExtra(int from = 0)
        {
            
            foreach (OneDayNews oneDayNews in latestNews.Skip(from).Take(latestNews.Count - from)) {
                int count = oneDayNews.News.Count;
                for (int i = 0; i < count; ++i) {
                    NewsItem itemTmp = oneDayNews.News[i];
                    if (await itemTmp.downloadExtra() == false)
                    {
                        return false;
                    }
                    oneDayNews.News[i] = itemTmp;
                }
            }
            return true;
        }
    }

    /* Holds Info for one day's news */
    class OneDayNews
    {
        public DateTime date = new DateTime();         // date
        public string dateString{get; set;}
        private ZhihuDailyXMLParser parser = new ZhihuDailyXMLParser();
        public ObservableCollection<NewsItem> News { get; private set; }
        public OneDayNews(DateTime date)
        {
            News = new ObservableCollection<NewsItem>();
            this.date = date;
            dateString = date.Date.ToString("yy/MM/dd");
        }

        /* update this day's news */
        public async Task<bool> updateNews(bool isOnlineDownload = false)
        {
            List<Dictionary<string, string>> newsList = await parser.getTitles(date, isOnlineDownload);
            if (newsList == null)
                return false;
            foreach (Dictionary<string, string> newsItem in newsList)
            {
                NewsItem item = new NewsItem();
                item.image_source = newsItem["image_source"];
                item.title = newsItem["title"];
                item.image = newsItem["image"];
                item.share_url = newsItem["share_url"];
                item.url = newsItem["url"];
                item.thumbnail = newsItem["thumbnail"];
                News.Add(item);
            }
            return true;
        }
    }

    /* Holds info for a piece of news */
    public class NewsItem{
        public string image_source { get; set;}
        public string image { get; set; }
        public string share_url { get; set; }
        public string url { get; set; }
        public string thumbnail { get; set; }
        public string content { get; set; }
        public string title{get; set;}
        public ImageSource image_bitmap { get; set; }
        public async Task<bool> downloadExtra()
        {
            ZhihuDailyFileManager fm = new ZhihuDailyFileManager();
            // download image and update image_update
            string filename = await fm.downloadFile(image);
            if (filename == null)
                return false;
            image_bitmap = await fm.LoadImage(filename);
            return true;
        }

        // get the news content 
        public async Task getContent()
        {
            ZhihuDailyXMLParser zn = new ZhihuDailyXMLParser();
            Dictionary<string, string> contentXML = await zn.getNewsContent(this.url);
            if (contentXML == null)
                return;
            this.content = contentXML["content"];
        }
    }
}
