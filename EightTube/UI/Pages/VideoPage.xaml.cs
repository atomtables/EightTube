using EightTube.API;
using EightTube.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class VideoPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        private string videoId;
        private Video video;

        public VideoPage()
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
            videoId = (string)e.NavigationParameter;
            await Invidious.catchErrors(async delegate ()
            {
                video = await Invidious.load<Video>($"Fetching data for video {videoId}...", Preferences.Video, videoId);
                DefaultViewModel["Video"] = video; //("JRPn7U_KfT0");
            }, this.CommandInvokedHandler, delegate (IUICommand _) { Frame.GoBack(); });
        }

        private async void CommandInvokedHandler(IUICommand command)
        {
            await Invidious.catchErrors(async delegate ()
            {
                video = await Invidious.load<Video>($"Fetching data for video {videoId}...", Preferences.Video, videoId);
                DefaultViewModel["Video"] = video; //("JRPn7U_KfT0");
            }, this.CommandInvokedHandler, delegate (IUICommand _) { Frame.GoBack(); });
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

        private void Comment_ItemClick(object sender, ItemClickEventArgs e)
        {
            var comment = ((Comment)e.ClickedItem);
            if (!Frame.Navigate(typeof(CommentPage), new Tuple<String, Comment>(videoId, comment)))
            {
                throw new Exception("Navigation Failed");
            }
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

        private void ChannelButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Frame.Navigate(typeof(ChannelPage), video.authorId))
            {
                throw new Exception("Navigation Failed");
            }
        }

        private async void PlayVideoButton_Click(object sender, RoutedEventArgs e)
        {
            VideoQualityDialog dialog = new VideoQualityDialog();
            List<string> l1 = new List<string>();
            foreach (FormatStream f in video.formatStreams)
            {
                l1.Add($"{f.container}: quality {f.quality}, bitrate {f.bitrate}");
            }
            dialog.SetItems(l1);
            await dialog.ShowAsync();
            System.Diagnostics.Debug.WriteLine($"result: {dialog.Result}, index: {dialog.Index}");
            if (dialog.Result == ContentDialogResult.Primary) // user responded properly
            {
                if (!Frame.Navigate(typeof(VideoViewerPage), video.formatStreams[dialog.Index].url))
                {
                    throw new Exception("Navigation failed");
                }
            }
        }

        private async void CommentsListView_Loaded(object sender, RoutedEventArgs e)
        {
            await Invidious.catchErrors(async delegate ()
            {
                CommentsData comments = await Invidious.load<CommentsData>($"Fetching comments for video {videoId}...", Preferences.Comments, videoId);
                comments.comments = Comment.Sort(comments.comments);
                DefaultViewModel["Comments"] = comments; //("JRPn7U_KfT0");
                DefaultViewModel["EnableCommentLoadButton"] = comments.continuation != null;
            }, this.CommentsUICommand);
        }

        private async void CommentsUICommand(IUICommand command)
        {
            await Invidious.catchErrors(async delegate ()
            {
                CommentsData comments = await Invidious.load<CommentsData>($"Fetching comments for video {videoId}...", Preferences.Comments, videoId);
                comments.comments = Comment.Sort(comments.comments);
                DefaultViewModel["Comments"] = comments; //("JRPn7U_KfT0");
                DefaultViewModel["EnableCommentLoadButton"] = comments.continuation != null;
            }, this.CommentsUICommand);
        }

        private async void CommentsButton_Click(object sender, RoutedEventArgs e)
        {
            await Invidious.catchErrors(async delegate ()
            {
                CommentsData currentCommentsState = (CommentsData)DefaultViewModel["Comments"];
                CommentsData comments = await Invidious.load<CommentsData>($"Fetching comments for video {videoId}...", Preferences.Comments, videoId, new Tuple<string, string>("continuation", currentCommentsState.continuation));

                currentCommentsState.comments = currentCommentsState.comments.Concat(comments.comments).ToList();
                currentCommentsState.comments = Comment.Sort(currentCommentsState.comments);
                currentCommentsState.continuation = comments.continuation;

                DefaultViewModel["Comments"] = currentCommentsState; //("JRPn7U_KfT0");
                DefaultViewModel["EnableCommentLoadButton"] = comments.continuation != null;
            });
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
