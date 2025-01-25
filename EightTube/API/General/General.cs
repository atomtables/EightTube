using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EightTube.API
{
    [DataContract]
    public class Image
    {
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public int width { get; set; }
        [DataMember]
        public int height { get; set; }

        [IgnoreDataMember]
        public Uri URL {
            get {
                Uri uri = new Uri(url);
                if (!uri.IsWellFormedOriginalString())
                {
                     uri = new Uri("https:" + url);
                }
                if (!uri.IsWellFormedOriginalString())
                {
                    uri = new Uri(Preferences.API + url);
                }
                return uri;
            }
        }
    }

    [DataContract]
    public class Thumbnail
    {
        [DataMember]
        public string quality { get; set; }
        [DataMember]
        string url { get; set; }
        [DataMember]
        public int width { get; set; }
        [DataMember]
        public int height { get; set; }

        [IgnoreDataMember]
        public Uri URL
        {
            get
            {
                Uri uri = new Uri(url);
                if (!uri.IsWellFormedOriginalString())
                {
                    uri = new Uri("https:" + url);
                }
                if (!uri.IsWellFormedOriginalString())
                {
                    uri = new Uri(Preferences.API + url);
                }
                System.Diagnostics.Debug.WriteLine(uri.OriginalString);
                return uri;
            }
        }
    }

}
