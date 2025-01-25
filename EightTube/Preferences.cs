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
        public static string API = "https://invidious.nerdvpn.de";

        // ok i cheated but its not my fault
        public static string Popular { get { return $"https://invidious.nerdvpn.de/api/v1/popular"; } }
        public static string Trending { get { return $"{API}/api/v1/trending"; } }
        public static string Video { get { return $"{API}/api/v1/videos"; } }
        public static string Comments { get { return $"{API}/api/v1/comments"; } }
        public static string Channel { get { return $"{API}/api/v1/channels"; } }
        public static string Search { get { return $"{API}/api/v1/search"; } }

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

