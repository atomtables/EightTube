using EightTube.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;

namespace EightTube.API
{
    [DataContract]
    public class PopularVideo: BaseVideo
    {
        [DataMember]
        public override string type { get; set; } // Will always be "shortVideo"

        [DataMember]
        public string title { get; set; }
        [DataMember]
        public override string videoId { get; set; }
        [DataMember]
        public override List<Thumbnail> videoThumbnails { get; set; }

        [DataMember]
        public override int lengthSeconds { get; set; }
        [DataMember]
        public override long viewCount { get; set; }

        [DataMember]
        public string author { get; set; }
        [DataMember]
        public string authorId { get; set; }
        [DataMember]
        public override string authorUrl { get; set; }

        [DataMember]
        public override long? published { get; set; }
        [DataMember]
        public string publishedText { get; set; }
    }
}
