using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ZhihuDailyForWindows8
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // ZhihuDailyFileManager fm = new ZhihuDailyFileManager();
        // private ZhihuDailyXMLParser xp = new ZhihuDailyXMLParser();
        private ZhihuNews zn = new ZhihuNews();
        private CollectionViewSource newsCollection;
        private int days = 0;
        public MainPage()
        {
            this.InitializeComponent();
            Window.Current.SizeChanged += Current_SizeChanged;
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == Windows.UI.Xaml.Navigation.NavigationMode.Back)
                return;
            /* Load info */
            newsCollection = new CollectionViewSource();
            loadingProgress.Visibility = Windows.UI.Xaml.Visibility.Visible;
            if (await zn.loadNews(DateTime.Today) == false)
            {
                var messageDialog = new Windows.UI.Popups.MessageDialog("网络错误，请检查您的网络连接再重新尝试该操作");
                var result = messageDialog.ShowAsync();
                loadingProgress.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                return;
            }
            newsCollection.IsSourceGrouped = true;
            newsCollection.Source = zn.latestNews;
            newsCollection.ItemsPath = new PropertyPath("News");
            newsContent.ItemsSource = newsCollection.View;
            newsContentListView.ItemsSource = newsCollection.View;
            if (await zn.loadExtra() == false)
            {
                var messageDialog = new Windows.UI.Popups.MessageDialog("网络错误，请检查您的网络连接再重新尝试该操作");
                var result = messageDialog.ShowAsync();
                loadingProgress.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                return;
            }
            loadingProgress.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            days = 1;
        }

        // click item
        private void newsContent_ItemClick_News(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem != null)
            {
                string url = ((NewsItem)e.ClickedItem).url;
                this.Frame.Navigate(typeof(NewsDetail), url);
            }
        }

        // refresh content
        private async void Button_Click_refresh(object sender, RoutedEventArgs e)
        {
            /* Load info */
            bottomAppBar.IsOpen = false;

            newsCollection = new CollectionViewSource();
            loadingProgress.Visibility = Windows.UI.Xaml.Visibility.Visible;
            if (await zn.loadNews(DateTime.Today) == false)
            {
                var messageDialog = new Windows.UI.Popups.MessageDialog("网络错误，请检查您的网络连接再重新尝试该操作");
                var result = messageDialog.ShowAsync();
                loadingProgress.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                return;
            }
            newsCollection.IsSourceGrouped = true;
            newsCollection.Source = zn.latestNews;
            newsCollection.ItemsPath = new PropertyPath("News");

            newsContent.ItemsSource = newsCollection.View;
            newsContentListView.ItemsSource = newsCollection.View;
            if (await zn.loadExtra() == false)
            {
                var messageDialog = new Windows.UI.Popups.MessageDialog("网络错误，请检查您的网络连接再重新尝试该操作");
                var result = messageDialog.ShowAsync();
                loadingProgress.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                return;
            }
            loadingProgress.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        // loading more items
        private async void Button_Click_loadMore(object sender, RoutedEventArgs e)
        {
            loadingProgress.Visibility = Windows.UI.Xaml.Visibility.Visible;
            if (await zn.loadNews(DateTime.Today.AddDays(-1.0 * days)) == false)
            {
                var messageDialog = new Windows.UI.Popups.MessageDialog("网络错误，请检查您的网络连接再重新尝试该操作");
                var result = messageDialog.ShowAsync();
                loadingProgress.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                return;
            }
            newsCollection.Source = zn.latestNews;
            newsCollection.ItemsPath = new PropertyPath("News");
            newsContent.ItemsSource = newsCollection.View;
            newsContentListView.ItemsSource = newsCollection.View;
            if (await zn.loadExtra(zn.latestNews.Count - 1) == false)
            {
                var messageDialog = new Windows.UI.Popups.MessageDialog("网络错误，请检查您的网络连接再重新尝试该操作");
                var result = messageDialog.ShowAsync();
                loadingProgress.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                return;
            }
            loadingProgress.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            ++days;
        }

        // change the visualstate when the size of window is changed
        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            switch (ApplicationView.Value)
            {
                case ApplicationViewState.Filled:
                    VisualStateManager.GoToState(this, "Filled", false);
                    break;

                case ApplicationViewState.FullScreenLandscape:
                    VisualStateManager.GoToState(this, "FullScreenLandscape", false);
                    break;

                case ApplicationViewState.Snapped:
                    VisualStateManager.GoToState(this, "Snapped", false);
                    break;

                case ApplicationViewState.FullScreenPortrait:
                    VisualStateManager.GoToState(this, "FullScreenPortrait", false);
                    break;

                default:
                    return;
            }
        }
    }
}