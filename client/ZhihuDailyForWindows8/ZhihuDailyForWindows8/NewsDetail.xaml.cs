using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.ApplicationModel;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace ZhihuDailyForWindows8
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class NewsDetail : ZhihuDailyForWindows8.Common.LayoutAwarePage
    {
        private Dictionary<string, string> contentXML;
        public NewsDetail()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected async override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // get the news content
            string url = (string)navigationParameter;
            ZhihuDailyXMLParser zx = new ZhihuDailyXMLParser();
            contentXML = await zx.getNewsContent(url);
            newsTitle.Text = contentXML["title"];
            string htmlCode = contentXML["body"];
            wv.NavigateToString(htmlCode);

            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager,
                DataRequestedEventArgs>(this.ShareImageHandler);

        }

        private void RegisterForShare()
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager,
                DataRequestedEventArgs>(this.ShareImageHandler);
        }

        private async void ShareImageHandler(DataTransferManager sender,
    DataRequestedEventArgs e)
        {
            DataRequest request = e.Request;
            request.Data.Properties.Title = newsTitle.Text + "(分享自 @知乎日报) " + contentXML["share_url"];
            request.Data.Properties.Description = newsTitle.Text + "(分享自 @知乎日报) " + contentXML["share_url"];

            // Because we are making async calls in the DataRequested event handler,
            //  we need to get the deferral first.
            DataRequestDeferral deferral = request.GetDeferral();

            // Make sure we always call Complete on the deferral.
            try
            {
                ZhihuDailyFileManager fm = new ZhihuDailyFileManager();
                /*string filename = await fm.downloadFile(contentXML["thumbnail"]);
                StorageFile thumbnailFile =
                    await Package.Current.InstalledLocation.GetFileAsync(filename)  ;
                request.Data.Properties.Thumbnail =
                    RandomAccessStreamReference.CreateFromFile(thumbnailFile);*/
                string filename = await fm.downloadFile(contentXML["share_image"]);
                if (filename != null)
                {
                    StorageFile imageFile =
                        await ApplicationData.Current.LocalFolder.GetFileAsync(filename);
                    request.Data.SetBitmap(RandomAccessStreamReference.CreateFromFile(imageFile));
                }
                else
                {
                    return;
                }
            }
            finally
            {
                deferral.Complete();
            }
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }
    }
}
