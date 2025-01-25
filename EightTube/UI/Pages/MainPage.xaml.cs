using EightTube.Common;
using EightTube.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;

// The Pivot Application template is documented at http://go.microsoft.com/fwlink/?LinkID=391641

namespace EightTube.UI
{
    public sealed partial class MainPage : Page
    {
        private const string FirstGroupName = "Popular";
        private const string SecondGroupName = "Trending";
        private const string ThirdGroupName = "Suscriptions";

        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>.
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            //var sampleDataGroup = await SampleDataSource.GetGroupAsync("Group-1");
            DefaultViewModel[FirstGroupName] = e.PageState?[FirstGroupName];
            DefaultViewModel[SecondGroupName] = e.PageState?[SecondGroupName];
        }

        private async void FirstPivot_Loaded(object sender, RoutedEventArgs e)
        {
            await Invidious.catchErrors(async delegate ()
            {
                this.DefaultViewModel[FirstGroupName] = await Invidious.load<List<PopularVideo>>("Fetching popular videos...", Preferences.Popular);
            }, PopularVideoDelegate);
        }

        private async void PopularVideoDelegate(IUICommand command)
        {
            await Invidious.catchErrors(async delegate ()
            {
                this.DefaultViewModel[FirstGroupName] = await Invidious.load<List<PopularVideo>>("Fetching popular videos...", Preferences.Popular);
            }, PopularVideoDelegate);
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache. Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/>.</param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            e.PageState[FirstGroupName] = DefaultViewModel[FirstGroupName];
            e.PageState[SecondGroupName] = DefaultViewModel[SecondGroupName];
        }

        /// <summary>
        /// Adds an item to the list when the app bar button is clicked.
        /// </summary>
        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchDialog dialog = new SearchDialog();
            await dialog.ShowAsync();
            System.Diagnostics.Debug.WriteLine($"result: {dialog.Result}, query: {dialog.Query}");
            if (dialog.Result == ContentDialogResult.Primary) // user responded properly
            {
                if (!Frame.Navigate(typeof(SearchResultsPage), new Tuple<string, int>(dialog.Query, dialog.Type)))
                {
                    throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
                }
            }
        }

        // the quick fox jumps over the lasy dog.

        /// <summary>
        /// Invoked when an item within a section is clicked.
        /// </summary>
        private void Video_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            var videoId = ((BaseVideo)e.ClickedItem).videoId;
            if (!Frame.Navigate(typeof(VideoPage), videoId))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        /// <summary>
        /// Loads the content for the second pivot item when it is scrolled into view.
        /// </summary>
        private async void SecondPivot_Loaded(object sender, RoutedEventArgs e)
        {
            //var sampleDataGroup = await SampleDataSource.GetGroupAsync("Group-2");
            await Invidious.catchErrors(async delegate ()
            {
                this.DefaultViewModel[SecondGroupName] = await Invidious.load<List<TrendingVideo>>("Fetching trending videos...", Preferences.Trending);
            }, TrendingVideoDelegate);
        }

        private async void ThirdPivot_Loaded(object sender, RoutedEventArgs e)
        {
            List<Channel> subscriptions = new List<Channel>();
            await Invidious.catchErrors(async delegate ()
            {
                foreach (Tuple<string, string> t in Preferences.Subscriptions)
                {
                    subscriptions.Add(await Invidious.load<Channel>($"Fetching data for channel {t.Item2}", Preferences.Channel, t.Item2));
                }
            }, SubscriptionsVideoDelegate);
            DefaultViewModel["Subscriptions"] = subscriptions;
        }

        private void Channel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!Frame.Navigate(typeof(ChannelPage), ((Channel)e.ClickedItem).authorId))
            {
                throw new Exception("Navigation Failed");
            }
        }

        private async void TrendingVideoDelegate(IUICommand command)
        {
            await Invidious.catchErrors(async delegate ()
            {
                this.DefaultViewModel[SecondGroupName] = await Invidious.load<List<TrendingVideo>>("Fetching trending videos...", Preferences.Trending);
            }, TrendingVideoDelegate);
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Frame.Navigate(typeof(SettingsPage)))
            {
                throw new Exception("Navigation failed");
            }
        }

        private async void SubscriptionsVideoDelegate(IUICommand command)
        {
            List<Channel> subscriptions = new List<Channel>();
            await Invidious.catchErrors(async delegate ()
            {
                foreach (Tuple<string, string> t in Preferences.Subscriptions)
                {
                    subscriptions.Add(await Invidious.load<Channel>($"Fetching data for channel {t.Item2}", Preferences.Channel, t.Item1));
                }
            }, SubscriptionsVideoDelegate);
            DefaultViewModel["Subscriptions"] = subscriptions;
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}
