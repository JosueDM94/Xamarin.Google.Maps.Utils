using System;

using Newtonsoft.Json;

namespace Sample.Droid.Models
{
    public class Position
    {
        [JsonProperty(PropertyName = "lat")]
        public double Latitude { get; set; }

        [JsonProperty(PropertyName = "lng")]
        public double Longitude { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "snippet")]
        public string Snippet { get; set; }
    }
}
