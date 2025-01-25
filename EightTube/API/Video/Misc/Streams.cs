using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EightTube.API
{
    [DataContract]
    public class AdaptiveFormat
    {
        [DataMember]
        public string index { get; set; }
        [DataMember]
        public string bitrate { get; set; }
        [DataMember]
        public string init { get; set; }
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public string itag { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public string clen { get; set; }
        [DataMember]
        public string projectionType { get; set; }
        [DataMember]
        public string container { get; set; }
        [DataMember]
        public string encoding { get; set; }
        [DataMember(IsRequired = false)]
        public string qualityLabel { get; set; }
        [DataMember(IsRequired = false)]
        public string resolution { get; set; }
        [DataMember]
        public int fps { get; set; }
        [DataMember(IsRequired = false)]
        public string size { get; set; }
        [DataMember(IsRequired = false)] 
        public long targetDurationsec { get; set; }
        [DataMember(IsRequired = false)]
        public long maxDvrDurationSec { get; set; }
        [DataMember(IsRequired = false)]
        public string audioQuality { get; set; }
        [DataMember(IsRequired = false)]
        public string audioChannels { get; set; }
        [DataMember(IsRequired = false)]
        public string colorInfo { get; set; }
        [DataMember(IsRequired = false)]
        public string captionTrack { get; set; }
    }

    [DataContract]
    public class FormatStream
    {
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public string itag { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public string quality { get; set; }
        [DataMember(IsRequired = false)]
        public string bitrate { get; set; }
        [DataMember]
        public string container { get; set; }
        [DataMember]
        public string encoding { get; set; }
        [DataMember]
        public string qualityLabel { get; set; }
        [DataMember]
        public string resolution { get; set; }
        [DataMember]
        public string size { get; set; }
    }
}
