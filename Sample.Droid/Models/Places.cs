using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sample.Droid.Models
{
    public class PlacesWrapper
    {
        [JsonProperty(PropertyName = "html_attributions")]
        public List<string> HtmlAttributions { get; set; }
        [JsonProperty(PropertyName = "results")]
        public List<Place> Place { get; set; }
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }
    }

    public class Place
    {
        [JsonProperty(PropertyName = "geometry")]
        public PLaceLocation Geometry { get; set; }
        [JsonProperty(PropertyName = "icon")]
        public string Icon { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "place_id")]
        public string PlaceId { get; set; }
        [JsonProperty(PropertyName = "types")]
        public List<string> Types { get; set; }
        [JsonProperty(PropertyName = "vicinity")]
        public string Vicinity { get; set; }
    }

    public class PLaceLocation{
        [JsonProperty(PropertyName = "location")]
        public Position Location { get; set; }
    }
}
