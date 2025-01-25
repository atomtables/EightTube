using EightTube.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace EightTube.API
{
    [DataContract]
    public class Searchable: Invidious
    {
        [DataMember]
        public virtual string type { get; set; }
    }


    public class Search: Invidious
    {
        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static async Task<List<Searchable>> loadQuery(string query, int type, int page = 1)
        {
            string t = (type == 0 ? "all" : type == 1 ? "video" : "channel");

            String st = await Invidious.load(
                $"Fetching results for search {query}...",
                $"{Preferences.Search}?q={query}&page={page}&type={t}"
            );
            List<Searchable> l1 = await DeserializeSearchResultsAsync(st);
            await SystemProgressIndicator.hide();
            return l1;
        }

        public static async Task<List<Searchable>> DeserializeSearchResultsAsync(String jsonResponse)
        {
            // Deserialize the JSON into a list of ISearchable, using a custom converter to handle the "type" field
            // var results = JsonConvert.DeserializeObject<List<ISearchable>>(jsonResponse);
            using (Stream st = GenerateStreamFromString(jsonResponse))
            {
                //var results = (List<Searchable>)new DataContractJsonSerializer(typeof(List<Searchable>)).ReadObject(st);
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    Converters = new[] { new SearchableConverter() }
                };

                List<Searchable> results = JsonConvert.DeserializeObject<List<Searchable>>(jsonResponse, settings);

                List<Searchable> searchableResults = new List<Searchable>();

                foreach (Searchable result in results)
                {
                    if (result is Video)
                    {
                        searchableResults.Add((Video)result);
                    }
                    else if (result is Channel)
                    {
                        searchableResults.Add((Channel)result);
                    }
                }

                return searchableResults;
            }
        }

    }

    public class SearchableConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Searchable) || objectType == typeof(List<Searchable>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(List<Searchable>))
            {
                JArray array = JArray.Load(reader);
                var searchables = new List<Searchable>();

                foreach (var item in array)
                {
                    string type = item["type"]?.ToString(); 

                    Searchable searchItem;
                    switch (type)
                    {
                        case "video":
                            searchItem = new Video();
                            break;
                        case "channel":
                            searchItem = new Channel();
                            break;
                        case "playlist":
                            continue;
                        default:
                            throw new Exception("Unknown type");
                    }
                    serializer.Populate(item.CreateReader(), searchItem);
                    searchables.Add(searchItem);
                }

                return searchables;
            }
            JObject jo = JObject.Load(reader);
            string singleType = jo["type"]?.ToString();
            Searchable searchable;

            switch (singleType)
            {
                case "video":
                    searchable = new Video();
                    break;
                case "channel":
                    searchable = new Channel();
                    break;
                case "playlist":
                    return null;
                default:
                    throw new Exception("Unknown type");
            }

            serializer.Populate(jo.CreateReader(), searchable);
            return searchable;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

}
