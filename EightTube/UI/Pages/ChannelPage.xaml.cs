using EightTube.API;
using EightTube.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace EightTube.UI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChannelPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        private string name;
        private string channelId;

        private ChannelVideos videoData;
        private ChannelVideos shortsData;
        private ChannelVideos streamsData;

        public ChannelPage()
        {
            this.InitializeComponent();

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
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            channelId = (string)e.NavigationParameter;
            await LoadChannelData();
        }

        private async void ChannelLoadCommand(IUICommand command)
        {
            await LoadChannelData();
        }

        private void Video_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            var videoId = ((BaseVideo)e.ClickedItem).videoId;
            if (!Frame.Navigate(typeof(VideoPage), videoId))
            {
                throw new Exception("Navigation Failed");
            }
        }

        private async Task LoadChannelData()
        {
            await Invidious.catchErrors(async delegate () {
                Channel channel = await Invidious.load<Channel>
                                    ($"Fetching data for channel {channelId}", Preferences.Channel, channelId);
                this.name = channel.author;
                DefaultViewModel["ChannelID"] = channelId;
                DefaultViewModel["Banner"] = channel.authorBanners.Last();
                DefaultViewModel["Author"] = channel.author;
                DefaultViewModel["Thumbnail"] = channel.authorThumbnail;
                DefaultViewModel["SubCount"] = channel.subCountText;
                DefaultViewModel["FullSubCount"] = $"{channel.subCountText:n0}";
                // also bugged
                DefaultViewModel["TotalViews"] = Invidious.FormatNumber(channel.totalViews);
                DefaultViewModel["FullTotalViews"] = $"{channel.totalViews:n0}";
                DefaultViewModel["FamilyFriendly"] = channel.isFamilyFriendly ? "• Family Friendly" : "";

                DefaultViewModel["Description"] = channel.description;
                // returns zero probably due to a bug on invidious's side
                DefaultViewModel["JoinedDate"] = channel.joinedDate.ToString();


                this.videoData = await Channel.loadVideos(channelId);
                DefaultViewModel["Videos"] = videoData.videos;
                DefaultViewModel["VideoLoadMore"] = videoData.continuation != null;
                this.shortsData = await Channel.loadShorts(channelId);
                DefaultViewModel["Shorts"] = shortsData.videos;
                DefaultViewModel["ShortLoadMore"] = shortsData.continuation != null;
                this.streamsData = await Channel.loadStreams(channelId);
                DefaultViewModel["Streams"] = streamsData.videos;
                DefaultViewModel["StreamLoadMore"] = shortsData.continuation != null;

                DefaultViewModel["AllowSubscription"] = true;
                DefaultViewModel["Subscribed"] = Preferences.Subscriptions.Exists(delegate (Tuple<string, string> t) { return t.Item2 == channelId; }) ? Visibility.Visible : Visibility.Collapsed;
                DefaultViewModel["Unsubscribed"] = Preferences.Subscriptions.Exists(delegate (Tuple<string, string> t) { return t.Item2 == channelId; }) ? Visibility.Collapsed : Visibility.Visible;


            }, ChannelLoadCommand, delegate (IUICommand _) { Frame.GoBack(); });
        }

        private async void VideoLoadMore_Click(object sender, RoutedEventArgs e) 
        {
            await Invidious.catchErrors(async delegate () {
                ChannelVideos c = await Channel.loadVideos(channelId, videoData.continuation);
                videoData.videos.AddRange(c.videos);
                videoData.continuation = c.continuation;

                DefaultViewModel["Videos"] = videoData.videos;
                DefaultViewModel["VideoLoadMore"] = videoData.continuation != null;
            });
        }

        private async void ShortLoadMore_Click(object sender, RoutedEventArgs e)
        {
            await Invidious.catchErrors(async delegate () {
                ChannelVideos c = await Channel.loadShorts(channelId, shortsData.continuation);
                shortsData.videos.AddRange(c.videos);
                shortsData.continuation = c.continuation;

                DefaultViewModel["Shorts"] = shortsData.videos;
                DefaultViewModel["ShortLoadMore"] = shortsData.continuation != null;
            });
        }

        private async void StreamLoadMore_Click(object sender, RoutedEventArgs e)
        {
            await Invidious.catchErrors(async delegate () {
                ChannelVideos c = await Channel.loadStreams(channelId, streamsData.continuation);
                streamsData.videos.AddRange(c.videos);
                streamsData.continuation = c.continuation;

                DefaultViewModel["Streams"] = streamsData.videos;
                DefaultViewModel["StreamLoadMore"] = streamsData.continuation != null;
            });
        }

        private void SubscribeButton_Click(object sender, RoutedEventArgs e)
        {
            if (Preferences.Subscriptions.Exists(delegate (Tuple<string, string> t) { return t.Item2 == channelId; }))
            {
                Preferences.Subscriptions.RemoveAt(Preferences.Subscriptions.FindIndex(delegate (Tuple<string, string> t) { return t.Item2 == channelId; }));
                Preferences.RefreshSubscriptionsList();
            }
            else
            {
                Preferences.Subscriptions.Add(new Tuple<string, string>(name, channelId));
                Preferences.RefreshSubscriptionsList();
            }
            DefaultViewModel["Subscribed"] = Preferences.Subscriptions.Exists(delegate (Tuple<string, string> t) { return t.Item2 == channelId; }) ? Visibility.Visible : Visibility.Collapsed;
            DefaultViewModel["Unsubscribed"] = Preferences.Subscriptions.Exists(delegate (Tuple<string, string> t) { return t.Item2 == channelId; }) ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
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
