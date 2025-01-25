using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EightTube.API
{
    [DataContract]
    public class Caption
    {
        [DataMember]
        public string label { get; set; }

        [DataMember(Name = "language_code")]
        public string languageCode { get; set; }

        [DataMember]
        string url { get; set; }

        [IgnoreDataMember]
        public Uri URL { get { return new Uri(url); } }
    }

    [DataContract]
    public class MusicTrack
    {
        [DataMember]
        public string song { get; set; }
        
        [DataMember]
        public string artist { get; set; }

        [DataMember]
        public string album { get; set; }

        [DataMember]
        public string license { get; set; }
    }
}
