using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EightTube.API
{
    [DataContract]
    public class RecommendedVideo: BaseVideo
    {
        [DataMember]
        public override string videoId { get; set; }

        [DataMember]
        public string title { get; set; }

        [DataMember]
        public override List<Thumbnail> videoThumbnails { get; set; }

        [DataMember]
        public string author { get; set; }
        [DataMember]
        public override string authorUrl { get; set; }
        [DataMember(IsRequired = false)]
        public string authorId { get; set; }
        [DataMember]
        public bool authorVerified { get; set; }
        [DataMember]
        public List<Image> authorThumbnails { get; set; }

        [DataMember]
        public override int lengthSeconds { get; set; }
        [DataMember]
        public override long viewCount { get; set; }
        [DataMember]
        public string viewCountText { get; set; }

        [DataMember]
        public override long? published { get; set; }
    }
}
