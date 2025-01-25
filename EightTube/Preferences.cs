using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace EightTube
{
    class Preferences
    {
        // built in support for invidious.nerdvpn.de, build changing this IP address.
        public static string API = "http://192.168.1.6:3000";

        // ok i cheated but its not my fault
        public static readonly string Popular = $"https://invidious.nerdvpn.de/api/v1/popular";
        public static readonly string Trending = $"{API}/api/v1/trending";
        public static readonly string Video = $"{API}/api/v1/videos";
        public static readonly string Comments = $"{API}/api/v1/comments";
        public static readonly string Channel = $"{API}/api/v1/channels";
        public static readonly string Search = $"{API}/api/v1/search";

        public static List<Tuple<String, String>> Subscriptions = new List<Tuple<string, string>>();
        public static void RefreshSubscriptionsList()
        {
            var subscriptionList = Preferences.Subscriptions.Select(t => new
            {
                name = t.Item1,
                id = t.Item2
            }).ToList();
            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(subscriptionList);
            Windows.Storage.ApplicationData.Current.LocalSettings.Values["Subscriptions"] = jsonString;
        }
        
        public static HttpClient Client
        {
            get
            {
                HttpClient client = new HttpClient();

                client.DefaultRequestHeaders.UserAgent.Clear();
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("EightTube", "1.0"));

                return client;
            }
        }
        
        

    }
}

