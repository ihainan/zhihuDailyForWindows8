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
        ScrollViewer scrollViewer;
        private ZhihuNews zn = new ZhihuNews();
        private CollectionViewSource newsCollection;
        private bool isEnableUpdateOrLoadDate = true;
        public double maxNum;        // use to load more data 
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
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            isEnableUpdateOrLoadDate = true;
            if (e.NavigationMode == Windows.UI.Xaml.Navigation.NavigationMode.Back)
                return;
            newsCollection = new CollectionViewSource();
            newsCollection.IsSourceGrouped = true;
            newsCollection.Source = zn.latestNews;
            newsCollection.ItemsPath = new PropertyPath("News");
            this.loadMoreNewsItems(false);
        }

        /* click item */
        private void newsContent_ItemClick_News(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem != null)
            {
                string url = ((NewsItem)e.ClickedItem).url;
                this.Frame.Navigate(typeof(NewsDetail), url);
            }
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

        private void newsContent_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                scrollViewer = this.FindVisualChildByName<ScrollViewer>(this.newsContent, "ScrollViewer");
                var scroollBar = this.FindVisualChildByName<ScrollBar>(scrollViewer, "HorizontalScrollBar");
               
                scroollBar.ValueChanged += scroollBar_ValueChanged;
            }
            catch (Exception error)
            {
                return;
            }
        }

        /// <summary>
        /// find visual control by name
        /// Author : 你不知未来你多么强大
        /// Website :http://blog.csdn.net/f10_s/
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public T FindVisualChildByName<T>(DependencyObject parent, string name) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                string controlName = child.GetValue(Control.NameProperty) as string;
                if (controlName == name)
                {
                    return child as T;
                }
                else
                {
                    T result = FindVisualChildByName<T>(child, name);
                    if (result != null)
                        return result;
                }
            }
            return null;
        }

        /// <summary>
        /// load more when the scrollViewer scroll to the right of gridview
        /// Author : 你不知未来你多么强大
        /// Website :http://blog.csdn.net/f10_s/
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void scroollBar_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            ScrollBar scrollbar = sender as ScrollBar;
            if ((isEnableUpdateOrLoadDate && maxNum != scrollViewer.HorizontalOffset && scrollViewer.ScrollableWidth == scrollViewer.HorizontalOffset))
            {
                maxNum = scrollViewer.HorizontalOffset;
                this.loadMoreNewsItems();
            }
            if (isEnableUpdateOrLoadDate && e.NewValue == scrollbar.Minimum)
            {
                // this.updateNewsItems();
            }

        }

        /* load more items and update the interface */
        private async void loadMoreNewsItems(bool isDisplayProgressRing = true)
        {
            DateTime day;
            isEnableUpdateOrLoadDate = false;                   // disable another load request
            loadingProgress.Visibility = Windows.UI.Xaml.Visibility.Visible;
            if(isDisplayProgressRing)
                sp_loadmore.Visibility = Windows.UI.Xaml.Visibility.Visible;
            if (zn.latestNews.Count == 0)
                day = DateTime.Today;
            else
                day = zn.latestNews[zn.latestNews.Count - 1].date.AddDays(-1.0);
            if (await zn.loadNews(day) == false)
            {
                /* load error */
                var messageDialog = new Windows.UI.Popups.MessageDialog("网络错误，请检查您的网络连接再重新尝试该操作");
                var result = messageDialog.ShowAsync();
                loadingProgress.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                sp_loadmore.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                isEnableUpdateOrLoadDate = true;
                return;
            }

            /* hide progressBar after loading text finish and then display the text */
            sp_loadmore.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            newsCollection.Source = zn.latestNews;
            newsCollection.ItemsPath = new PropertyPath("News");
            newsContent.ItemsSource = newsCollection.View;
            newsContentListView.ItemsSource = newsCollection.View;

            /* load images */
            if (await zn.loadExtra(zn.latestNews.Count - 1) == false)
            {
                var messageDialog = new Windows.UI.Popups.MessageDialog("网络错误，请检查您的网络连接再重新尝试该操作");
                var result = messageDialog.ShowAsync();
                loadingProgress.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                isEnableUpdateOrLoadDate = true;
                return;
            }

            /* finish */
            loadingProgress.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            isEnableUpdateOrLoadDate = true;
        }

        /* get the latest news and update the interface */
        private async void updateNewsItems()
        {
            isEnableUpdateOrLoadDate = false;           // disable another update request
            maxNum = 0;
            // jump to the left of scrollview
            loadingProgress.Visibility = Windows.UI.Xaml.Visibility.Visible;        // display progressBar and progressRing
            sp_refresh.Visibility = Windows.UI.Xaml.Visibility.Visible;
            if (await zn.updateNews(DateTime.Today) == false)
            {
                /* refresh error */
                var messageDialog = new Windows.UI.Popups.MessageDialog("网络错误，请检查您的网络连接再重新尝试该操作");
                var result = messageDialog.ShowAsync();
                loadingProgress.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                sp_refresh.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                isEnableUpdateOrLoadDate = true;
                return;
            }

            /* hide progressRing after loading text finish and then display the text */
            sp_refresh.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            newsCollection.IsSourceGrouped = true;
            newsCollection.Source = zn.latestNews;
            newsCollection.ItemsPath = new PropertyPath("News");
            newsContent.ItemsSource = newsCollection.View;
            newsContentListView.ItemsSource = newsCollection.View;

            /* load images */
            if (await zn.loadExtra() == false)
            {
                var messageDialog = new Windows.UI.Popups.MessageDialog("网络错误，请检查您的网络连接再重新尝试该操作");
                var result = messageDialog.ShowAsync();
                loadingProgress.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                isEnableUpdateOrLoadDate = true;
                return;
            }

            /* finish refresh */
            loadingProgress.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            isEnableUpdateOrLoadDate = true;
        }

    }
}