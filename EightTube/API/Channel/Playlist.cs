using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EightTube.API
{
    // no actual support because it never really shows up anywhere in the api
    [DataContract]
    class Playlist: Searchable
    {
        [DataMember]
        public override string type { get; set; } // playlist

        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string playlistId { get; set; }
        [DataMember]
        public string playlistThumbnail { get; set; }

        [DataMember]
        public string author { get; set; }
        [DataMember]
        public string authorId { get; set; }
        [DataMember]
        public string authorUrl { get; set; }
        [DataMember]
        public bool authorVerified { get; set; }

        [DataMember]
        public int videoCount { get; set; }
        [DataMember]
        public List<Video> videos { get; set; }
    }
}
