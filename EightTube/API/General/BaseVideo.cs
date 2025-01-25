using EightTube.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;

namespace EightTube.API
{
    [DataContract]
    public abstract class BaseVideo: Searchable
    {
        [DataMember(IsRequired = false)]
        public abstract long? published { get; set; }
        [IgnoreDataMember]
        public DateTime publishedDate { get { return ConvertEpochToDateTime(published ?? 0); } }
        [IgnoreDataMember]
        public string publishedFormattedDate
        {
            get
            {
                if (published == null) return "";
                return "Published " + FormatDate(publishedDate);
            }
        }

        [DataMember]
        public abstract string videoId { get; set; }

        [DataMember]
        public abstract List<Thumbnail> videoThumbnails { get; set; }

        [IgnoreDataMember]
        public Thumbnail optimalThumbnail
        {
            get
            {
                int width = (int)ApplicationView.GetForCurrentView().VisibleBounds.Width;
                for (int i = 0; i < videoThumbnails?.Count; i++)
                {
                    if (width > videoThumbnails[i].height 
                        && videoThumbnails[i].quality.Contains("maxresdefault")
                    )
                    {
                        return videoThumbnails[i];
                    }
                }
                return videoThumbnails?.GetRange(1, 1).Single();
            }
        }

        [DataMember]
        public abstract int lengthSeconds { get; set; }
        [IgnoreDataMember]
        public string formattedLength
        {
            get
            {
                string str = "";
                if (lengthSeconds >= 3600)
                {
                    str += lengthSeconds / 3600 + ":";
                    if ((lengthSeconds / 60) % 60 < 10)
                    {
                        str += "0" + (lengthSeconds / 60) % 60 + ":";
                    }
                    else
                    {
                        str += (lengthSeconds / 60) % 60 + ":";
                    }
                    if (lengthSeconds % 60 < 10)
                    {
                        str += "0" + lengthSeconds % 60;
                    }
                    else
                    {
                        str += lengthSeconds % 60;
                    }
                }
                else
                {
                    if ((lengthSeconds / 60) % 60 < 10)
                    {
                        str += "0" + (lengthSeconds / 60) % 60 + ":";
                    }
                    else
                    {
                        str += (lengthSeconds / 60) % 60 + ":";
                    }
                    if (lengthSeconds % 60 < 10)
                    {
                        str += "0" + lengthSeconds % 60;
                    }
                    else
                    {
                        str += lengthSeconds % 60;
                    }
                }
                return str;
            }
        }
        [DataMember]
        public abstract long viewCount { get; set; }
        [IgnoreDataMember]
        public string formattedViews
        {
            get
            {
                return FormatNumber(viewCount);
            }
        }

        [DataMember]
        public abstract string authorUrl { get; set; }
        // useless
        [IgnoreDataMember]
        public Uri authorURL
        {
            get
            {
                return null; // new Uri(authorUrl);
            }
        }
       
    }
}
