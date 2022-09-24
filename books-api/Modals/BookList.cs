using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace books_api.Modals
{
    public class BookList
    {
        [JsonProperty("items")]
        public List<Items> Items { get; set; }
    }
    public class Items
    {
        [JsonProperty("volumeInfo")]
        public VolumeInfo VolumeInfo { get; set; }
    }
    public class VolumeInfo
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("authors")]
        public List<string> Authors { get; set; }

        [JsonProperty("publisher")]
        public string Publisher { get; set; }

        [JsonProperty("publishedDate")]
        public string PublishedDate { get; set; }
    }
}
