using EightTube.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace EightTube.API
{

    [DataContract]
    public class Video : BaseVideo
    {
        [DataMember]
        public override string type { get; set; } // "video" | "published"

        [DataMember]
        public string title { get; set; }
        [DataMember]
        public override string videoId { get; set; }

        [DataMember]
        public override List<Thumbnail> videoThumbnails { get; set; }
        // unimplemented: public List<Storyboard> storyboards { get; set; }

        [DataMember]
        public string description { get; set; }
        [DataMember]
        public string descriptionHtml { get; set; }

        [DataMember]
        public override long? published { get; set; }
        [DataMember]
        public string publishedText { get; set; }

        [DataMember]
        public List<string> keywords { get; set; }
        [DataMember]
        public override long viewCount { get; set; }
        [DataMember]
        public long likeCount { get; set; }
        [DataMember]
        public int dislikeCount { get; set; }

        [IgnoreDataMember]
        public int likeRatio
        {
            get
            {
                return (int)(((double)likeCount / (double)viewCount) * 100);
            }
        }

        [DataMember]
        public bool paid { get; set; }
        [DataMember]
        public bool premium { get; set; }
        [DataMember]
        public bool isFamilyFriendly { get; set; }
        [DataMember]
        public List<string> allowedRegions { get; set; }
        [DataMember]
        public string genre { get; set; }
        [DataMember]
        public string genreUrl { get; set; }
        [IgnoreDataMember]
        public Uri genreURL { get { return new Uri(genreUrl); } }

        [DataMember]
        public string author { get; set; }
        [DataMember]
        public string authorId { get; set; }
        [DataMember]
        public override string authorUrl { get; set; }
        [DataMember]
        public bool authorVerified { get; set; }
        [DataMember]
        public List<Image> authorThumbnails { get; set; }
        [IgnoreDataMember]
        public Image authorThumbnail { get { return authorThumbnails.Last(); } }

        [DataMember]
        public string subCountText { get; set; }
        [DataMember]
        public override int lengthSeconds { get; set; }
        [DataMember]
        public bool allowRatings { get; set; }
        [DataMember]
        public float rating { get; set; }
        [DataMember]
        public bool isListed { get; set; }
        [DataMember]
        public bool liveNow { get; set; }
        [DataMember]
        public bool isPostLiveDvr { get; set; }
        [DataMember]
        public bool isUpcoming { get; set; }
        [DataMember(Name = "dashUrl:")]
        public string dashUrl { get; set; }
        [DataMember]
        public long premiereTimestamp { get; set; }

        [DataMember]
        public string hlsUrl { get; set; }
        [DataMember]
        public List<AdaptiveFormat> adaptiveFormats { get; set; }
        [DataMember]
        public List<FormatStream> formatStreams { get; set; }

        [DataMember]
        public List<Caption> captions { get; set; }
        [DataMember]
        public List<MusicTrack> musicTracks { get; set; }

        [DataMember]
        public List<RecommendedVideo> recommendedVideos { get; set; }
    }
}
