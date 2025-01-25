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
using Windows.UI.ViewManagement;

namespace EightTube.API
{
    [DataContract]
    public class TrendingVideo: BaseVideo
    {
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
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public string descriptionHtml { get; set; }

        [DataMember]
        public bool liveNow { get; set; }
        [DataMember]
        public bool paid { get; set; }
        [DataMember]
        public bool premium { get; set; }

        /*public static async Task<List<TrendingVideo>> load()
        {
            await SystemProgressIndicator.show("Fetching trending videos...");

            var result = new List<TrendingVideo>();

            try
            {
                HttpResponseMessage response = await Preferences.Client.GetAsync(Preferences.API + "/api/v1/trending");

                using (System.IO.Stream stream = await response.Content.ReadAsStreamAsync())
                {
                    result = (List<TrendingVideo>)new DataContractJsonSerializer(typeof(List<TrendingVideo>)).ReadObject(stream);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("There was some sort of error: " + e);
            }

            await SystemProgressIndicator.hide();

            return result;
        }*/
    }
}
