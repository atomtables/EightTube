using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EightTube.API
{
    [DataContract]
    public class VideoObject : BaseVideo
    {
        [DataMember]
        public override string type { get; set; } // will always be video

        [DataMember]
        public string title { get; set; }
        [DataMember]
        public override string videoId { get; set; }

        [DataMember]
        public string author { get; set; }
        [DataMember]
        public string authorId { get; set; }
        [DataMember]
        public override string authorUrl { get; set; }
        [DataMember]
        public string authorVerified { get; set; }

        [DataMember]
        public override List<Thumbnail> videoThumbnails { get; set; }

        [DataMember]
        public string description { get; set; }
        [DataMember]
        public string descriptionHtml { get; set; }

        [DataMember]
        public override long viewCount { get; set; }
        [DataMember]
        public string viewCountText { get; set; }
        [DataMember]
        public override int lengthSeconds { get; set; }

        [DataMember]
        public override long? published { get; set; }
        [DataMember]
        public string publishedText { get; set; }

        [DataMember]
        public long premiereTimestamp { get; set; }

        [DataMember]
        public bool liveNow { get; set; }
        [DataMember]
        public bool premium { get; set; }
        [DataMember]
        public bool isUpcoming { get; set; }

        [DataMember]
        public bool isNew { get; set; }
        [DataMember]
        public bool is4k { get; set; }
        [DataMember]
        public bool is8k { get; set; }
        [DataMember]
        public bool isVr180 { get; set; }
        [DataMember]
        public bool isVr360 { get; set; }
        [DataMember]
        public bool is3d { get; set; }
        [DataMember]
        public bool hasCaptions { get; set; }
    }
}
